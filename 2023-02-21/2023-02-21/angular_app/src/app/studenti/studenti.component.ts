import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Router} from "@angular/router";
declare function porukaSuccess(a: string):any;
declare function porukaError(a: string):any;

@Component({
  selector: 'app-studenti',
  templateUrl: './studenti.component.html',
  styleUrls: ['./studenti.component.css']
})
export class StudentiComponent implements OnInit {

  title:string = 'angularFIT2';
  ime_prezime:string = '';
  opstina: string = '';
  studentPodaci: any;
  filter_ime_prezime: boolean;
  filter_opstina: boolean;
  novi_student: any;
  opstinaPodaci: any;
  defaultOpstina: number;


  constructor(private httpKlijent: HttpClient, private router: Router) {
  }

  getStudenti() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Student/GetAll", MojConfig.http_opcije()).subscribe((x:any)=>{
      this.studentPodaci = x;
      this.defaultOpstina=x[0].opstina_rodjenja_id
    });
  }

  getOpstina() :void
  {
    this.httpKlijent.get(MojConfig.adresa_servera+ "/Opstina/GetByAll", MojConfig.http_opcije()).subscribe(x=>{
      this.opstinaPodaci = x;
    });
  }

  ngOnInit(): void {
    this.getStudenti();
    this.getOpstina()
  }

  filtrirajPodatke() {
    if(this.filter_ime_prezime || this.filter_opstina){
      return this.studentPodaci.filter((s:any)=>
        (//!this.filter_ime_prezime ||
          ((s.ime+" "+s.prezime).startsWith(this.ime_prezime) ||
            (s.prezime+" "+s.ime).startsWith(this.ime_prezime)) && s.isDeleted==false
        ) &&
        (//!this.filter_opstina ||
          (s.opstina_rodjenja.description).startsWith(this.opstina)  && s.isDeleted==false )

      )

    }
    return  this.studentPodaci?.filter((s:any)=>  s.isDeleted==false)
  }

  obrisi(id:number) {
    this.httpKlijent.delete(MojConfig.adresa_servera+ "/Student/Obrisi?id="+id, MojConfig.http_opcije()).subscribe(x=>{
      this.getStudenti()
    });
    porukaSuccess("uspjesno obrisano")
  }

  noviStudent() {
    this.novi_student={
      id:0,
      ime:this.ime_prezime,
      prezime:'',
      opstina_rodjenja_id:this.defaultOpstina
    }
  }


  odabrani_student: any;
  snimiAdd() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/Student/Snimi", this.novi_student,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.getStudenti()

    });
    this.novi_student=null
    porukaSuccess("uspjesno dodano")
  }

  snimiEdit() {
    this.httpKlijent.post(MojConfig.adresa_servera+ "/Student/Snimi", this.odabrani_student,MojConfig.http_opcije()).subscribe((x:any)=>{
      this.getStudenti()
    });
    this.odabrani_student=null
    porukaSuccess("uspjesno uređeno")
  }
}
