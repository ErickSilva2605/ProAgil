import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  // Variaveis
  eventos: Evento[];
  eventosFiltrados: Evento[];
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;

  // Propriedades
  FiltroLista: string;
  get filtroLista(): string {
    return this.FiltroLista;
  }
  set filtroLista(value: string) {
    this.FiltroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  constructor(private eventoService: EventoService, private modalService: BsModalService) { }

  ngOnInit() {
    this.getEventos();
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
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
