import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  // Variaveis
  eventos: Evento[];
  evento: Evento;
  eventosFiltrados: Evento[];
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  registerForm: FormGroup;

  // Propriedades
  FiltroLista: string;
  get filtroLista(): string {
    return this.FiltroLista;
  }
  set filtroLista(value: string) {
    this.FiltroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  constructor(
      private eventoService: EventoService
    , private modalService: BsModalService
    , private fb: FormBuilder
    , private localeService: BsLocaleService
    ) {
      this.localeService.use('pt-br');
     }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      data: ['', Validators.required],
      imagemURL: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      this.evento = Object.assign({}, this.registerForm.value);
      this.eventoService.postEvento(this.evento).subscribe(
        (novoEvento: Evento) => {
          console.log(novoEvento);
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        }
      );
    }
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      e => e.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      e.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (evento: Evento[]) => {
      this.eventos = evento;
      this.eventosFiltrados = this.eventos;
      }, error => {
        console.log(error);
      });
  }

}
