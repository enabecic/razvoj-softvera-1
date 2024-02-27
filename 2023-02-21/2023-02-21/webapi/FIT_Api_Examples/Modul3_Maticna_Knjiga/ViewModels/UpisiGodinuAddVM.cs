using System;

namespace FIT_Api_Examples.Modul3_Maticna_Knjiga.ViewModels
{
    public class UpisiGodinuAddVM
    {
        public DateTime DatumUpisa { get; set; }
        public int GodinaStudija { get; set; }
        public int AkademskaGodinaID { get; set; }
        public float CijenaSkolarine { get; set; }
        public bool Obnova { get; set; }
        public int StudentID { get; set; }
    }
}
