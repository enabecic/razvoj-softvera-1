using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

      

        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime)
        {
            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(x => ime_prezime == null || (x.ime + " " + x.prezime).StartsWith(ime_prezime) || (x.prezime + " " + x.ime).StartsWith(ime_prezime))
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(100).ToList();
        }

        [HttpDelete]
        public ActionResult Obrisi(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("niste logirani");


            var student = _dbContext.Student.Find(id);

            if (student == null)
                return BadRequest("pogresan id");

            student.isDeleted = true;
            _dbContext.SaveChanges();

            return Ok();
        }


        [HttpPost]
        public ActionResult<Student> Snimi(StudentAddVM obj)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("niste logirani");

            Student student;
            if (obj.id == 0)
            {
                student = new Student();
                _dbContext.Add(student);
                student.created_time = DateTime.Now;
                student.slika_korisnika = Config.SlikeURL + "empty.png";
            }
            else
            {
                student = _dbContext.Student.Find(obj.id);
                if (student == null)
                    return BadRequest("pogresan id");
            }

            student.ime = obj.ime;
            student.prezime = obj.prezime;
            student.opstina_rodjenja_id = obj.opstina_rodjenja_id;

            _dbContext.SaveChanges();

            if (student.broj_indeksa == null)
            {
                student.broj_indeksa = "IB240" + student.id;
                _dbContext.SaveChanges();
            }

            return Ok(student);
        }


        [HttpGet]
        public ActionResult<Student> GetAllById(int id)
        {
            var student = _dbContext.Student.Find(id);
            if (student == null)
                return BadRequest("pogresan id");
            return Ok(student);
        }


    }
}
