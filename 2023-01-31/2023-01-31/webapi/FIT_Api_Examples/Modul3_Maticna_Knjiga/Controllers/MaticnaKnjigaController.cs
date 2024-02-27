using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3_Maticna_Knjiga.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MaticnaKnjigaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public MaticnaKnjigaController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }



        [HttpGet]
        public List<UpisiGodinu> GetAll(int id)
        {
            return _dbContext.UpisiGodinu.Where(x => x.StudentID == id).Include(x => x.AkademskaGodina).ToList();
        }

        [HttpPost]

        public ActionResult Snimi(UpisiGodinuAddVM obj)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("niste logirani");

            var evidentirao = HttpContext.GetLoginInfo().korisnickiNalog.korisnickoIme;
            //var evidentirao = "ena";

            var pronadjen = _dbContext.UpisiGodinu.Where(x => x.StudentID == obj.StudentID && x.GodinaStudija == obj.GodinaStudija && !obj.Obnova).FirstOrDefault();

            if (pronadjen == null || obj.Obnova)
            {
                var ug = new UpisiGodinu()
                {
                    DatumUpisa=obj.DatumUpisa,
                    GodinaStudija=obj.GodinaStudija,
                    AkademskaGodinaID=obj.AkademskaGodinaID,
                    CijenaSkolarine=obj.CijenaSkolarine,
                    Obnova=obj.Obnova,
                    DatumOvjere=null,
                    Napomena="text",
                    Evidentirao=evidentirao,
                    StudentID=obj.StudentID
                };
                _dbContext.Add(ug);
                _dbContext.SaveChanges();
            }

            else
                return BadRequest("vec upisna godina");
            return Ok();

        }



        [HttpPost]

        public ActionResult Ovjeri(UpisiGodinuOvjeriVM obj)
        {
            //if (!HttpContext.GetLoginInfo().isLogiran)
            //    return BadRequest("niste logirani");
            var podatak = _dbContext.UpisiGodinu.Find(obj.Id);

            if (podatak == null)
                return BadRequest("pogresan id");

            podatak.Napomena = obj.Napomena;
            podatak.DatumOvjere = obj.DatumOvjere;

            _dbContext.SaveChanges();
            
            return Ok();

        }


    }
}
