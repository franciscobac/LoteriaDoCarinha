import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-consultar',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './consultar-concurso.html',
  styleUrls: ['./consultar-concurso.css']
})
export class ConsultarConcurso {
  loteria = 'megasena';
  concursoNumero?: number;
  carregando = false;
  resultado: any = null;
  erro: string | null = null;

  constructor(
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) {}

  buscar() {
    if (!this.concursoNumero) {
      alert('Digite o número do concurso');
      return;
    }

    // Resetar estados
    this.carregando = true;
    this.resultado = null;
    this.erro = null;
    
    // Forçar atualização da UI
    this.cdr.detectChanges();
    
    console.log('Buscando concurso:', this.loteria, this.concursoNumero);

    this.http.get(`http://localhost:5084/api/Concursos/buscar?loteria=${this.loteria}&concurso=${this.concursoNumero}`)
      .subscribe({
        next: (res: any) => {
          console.log('Dados recebidos:', res);
          this.resultado = res;
          this.carregando = false;
          this.cdr.detectChanges(); // Forçar detecção de mudanças
        },
        error: (err) => {
          console.error('Erro:', err);
          this.erro = 'Concurso não encontrado';
          this.carregando = false;
          this.cdr.detectChanges(); // Forçar detecção de mudanças
        }
      });
  }
}
