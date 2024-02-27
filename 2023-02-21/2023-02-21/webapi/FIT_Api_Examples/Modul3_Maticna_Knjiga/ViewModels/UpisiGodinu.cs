using FIT_Api_Examples.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Modul3_Maticna_Knjiga.ViewModels
{
    public class UpisiGodinu
    {
        public int Id { get; set; }
        public DateTime DatumUpisa { get; set; }
        public int GodinaStudija { get; set; }


        [ForeignKey(nameof(AkademskaGodinaID))]
        public AkademskaGodina AkademskaGodina { get; set; }
        public int AkademskaGodinaID { get; set; }


        public float CijenaSkolarine { get; set; }
        public bool Obnova { get; set; }
        public DateTime? DatumOvjere{ get; set; }
        public string Napomena { get; set; }
        public string Evidentirao { get; set; }



        [ForeignKey(nameof(StudentID))]
        public Student Student { get; set; }
        public int StudentID { get; set; }

    }
}
