import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {MojConfig} from "../moj-config";
import {HttpClient} from "@angular/common/http";

declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-student-maticnaknjiga',
  templateUrl: './student-maticnaknjiga.component.html',
  styleUrls: ['./student-maticnaknjiga.component.css']
})
export class StudentMaticnaknjigaComponent implements OnInit {
  student_id: any;
   studentPodaci: any;
  studentIme: any;
  studentPrezime: any;
  prikazi_upis: boolean=false;
  DatumUpisa: any;
  GodinaStudija: any;
  AkademskaGodinaID: any;
  akadesmkiPodaci: any;
  ovjereniPodaci: any;
  CijenaSkolarine: any;
  Obnova: any;
  prikazi_ovjeru: boolean=false;
  DatumOvjere: any;
  Napomena: any;
  idOvjere: any;

  constructor(private httpKlijent: HttpClient, private route: ActivatedRoute) {}

  ovjeriLjetni(s:any) {

  }

  upisLjetni(s:any) {

  }

  ovjeriZimski(s:any) {

  }

  ngOnInit(): void {
    this.student_id=this.route.snapshot.params["id"];
    this.getStudent()
    this.getAkademske()
    this.getOvjerene()
  }

  private getStudent() {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/GetAllById?id="+this.student_id, MojConfig.http_opcije()).subscribe(x=>{
      this.studentPodaci = x;
      this.studentIme=this.studentPodaci.ime
      this.studentPrezime=this.studentPodaci.prezime

    });
  }

  private getAkademske() {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/AkademskeGodine/GetAll_ForCmb", MojConfig.http_opcije()).subscribe(x=>{
      this.akadesmkiPodaci = x;
    });
  }

  private getOvjerene() {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/MaticnaKnjiga/GetAll?id="+this.student_id, MojConfig.http_opcije()).subscribe(x=>{
      this.ovjereniPodaci = x;
    });
  }

  snimiUpis() {

    var upis:upisInfo={
      datumUpisa:this.DatumUpisa,
      godinaStudija:this.GodinaStudija,
      akademskaGodinaID:this.AkademskaGodinaID,
      cijenaSkolarine:this.CijenaSkolarine,
      obnova:this.Obnova,
      studentID:this.student_id
    }

    this.httpKlijent.post(MojConfig.adresa_servera+ "/MaticnaKnjiga/Snimi", upis,MojConfig.http_opcije()).subscribe(x=>{
      this.getOvjerene()
    });
    this.prikazi_upis=false
    porukaSuccess("uspjesno dodano")
  }

  snimiOvjeru() {

    var ovjera:ovjeraInfo={
      id:this.idOvjere,
      napomena:this.Napomena,
      datumOvjere:this.DatumOvjere
    }
    this.httpKlijent.post(MojConfig.adresa_servera+ "/MaticnaKnjiga/Ovjeri", ovjera,MojConfig.http_opcije()).subscribe(x=>{
      this.getOvjerene()
    });
    this.prikazi_ovjeru=false
    porukaSuccess("uspjesno ovjereno")
  }
}

export interface upisInfo{
datumUpisa :string
godinaStudija :number
akademskaGodinaID:number
cijenaSkolarine :number
obnova :boolean
studentID :number
}
export interface ovjeraInfo{
  datumOvjere :string
  napomena:string
  id:number
}
