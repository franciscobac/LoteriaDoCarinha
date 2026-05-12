import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-consultar-concurso',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="card">
      <div class="card-header bg-info text-white">
        <h4 class="mb-0">Consultar Concurso</h4>
      </div>
      <div class="card-body">
        <div class="row">
          <div class="col-md-4">
            <label class="form-label">Loteria</label>
            <select class="form-select" [(ngModel)]="loteria">
              <option value="megasena">Mega-Sena</option>
              <option value="lotofacil">Lotofácil</option>
            </select>
          </div>
          <div class="col-md-4">
            <label class="form-label">Número do Concurso</label>
            <input type="number" class="form-control" [(ngModel)]="concursoNumero">
          </div>
          <div class="col-md-4">
            <label class="form-label">&nbsp;</label>
            <button class="btn btn-primary w-100" (click)="buscar()" [disabled]="carregando">
              {{ carregando ? 'Buscando...' : 'Buscar' }}
            </button>
          </div>
        </div>

        <div *ngIf="resultado" class="mt-4">
          <hr>
          <h5>Resultado do Concurso {{ resultado.concurso }}</h5>
          <div class="alert alert-success">
            📅 Data: {{ resultado.dataSorteio }}
          </div>
          <div class="d-flex flex-wrap gap-2">
            <span *ngFor="let num of resultado.numeros" class="badge bg-danger p-3 fs-5 rounded-circle">
              {{ num }}
            </span>
          </div>
        </div>

        <div *ngIf="!resultado && !carregando" class="mt-4 text-center text-muted">
          🔍 Digite o número do concurso e clique em Buscar
        </div>
      </div>
    </div>
  `
})
export class ConsultarConcursoComponent {
  loteria = 'megasena';
  concursoNumero?: number;
  carregando = false;
  resultado: any = null;

  buscar() {
    if (!this.concursoNumero) {
      alert('Digite o número do concurso');
      return;
    }

    this.carregando = true;
    
    // Simular busca na API
    setTimeout(() => {
      // Dados simulados
      this.resultado = {
        concurso: this.concursoNumero,
        dataSorteio: new Date().toLocaleDateString(),
        numeros: [1, 15, 23, 34, 45, 56]
      };
      this.carregando = false;
    }, 1000);
  }
}
