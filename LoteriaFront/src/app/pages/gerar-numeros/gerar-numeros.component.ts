import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-gerar-numeros',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="card">
      <div class="card-header bg-primary text-white">
        <h4 class="mb-0">Gerar Números para Aposta</h4>
      </div>
      <div class="card-body">
        <div class="row">
          <div class="col-md-6">
            <label class="form-label">Loteria</label>
            <select class="form-select" [(ngModel)]="tipoLoteriaId">
              <option [value]="1">Mega-Sena (6 números - 1 a 60)</option>
              <option [value]="2">Lotofácil (15 números - 1 a 25)</option>
            </select>
          </div>
          <div class="col-md-6">
            <label class="form-label">Observação (opcional)</label>
            <input type="text" class="form-control" [(ngModel)]="observacao" placeholder="Ex: Aposta para concurso 3000">
          </div>
        </div>
        <div class="mt-3">
          <button class="btn btn-primary" (click)="gerar()" [disabled]="carregando">
            {{ carregando ? 'Gerando...' : 'Gerar Números' }}
          </button>
        </div>

        <div *ngIf="numerosGerados" class="mt-4">
          <hr>
          <h5>Números Gerados - {{ tipoLoteriaId === 1 ? 'Mega-Sena' : 'Lotofácil' }}</h5>
          <div class="d-flex flex-wrap gap-2 mb-3">
            <span *ngFor="let num of numerosGerados" class="badge bg-success p-3 fs-5 rounded-circle">
              {{ num }}
            </span>
          </div>
          <div class="alert alert-success">
            ✅ Números salvos no seu histórico! (simulado)
          </div>
        </div>
      </div>
    </div>
  `
})
export class GerarNumerosComponent {
  tipoLoteriaId = 1;
  observacao = '';
  numerosGerados: number[] | null = null;
  carregando = false;

  gerar() {
    this.carregando = true;
    
    // Gerar números aleatórios
    setTimeout(() => {
      const quantidade = this.tipoLoteriaId === 1 ? 6 : 15;
      const maxNumero = this.tipoLoteriaId === 1 ? 60 : 25;
      const numeros = new Set<number>();
      
      while (numeros.size < quantidade) {
        const num = Math.floor(Math.random() * maxNumero) + 1;
        numeros.add(num);
      }
      
      this.numerosGerados = Array.from(numeros).sort((a, b) => a - b);
      this.carregando = false;
    }, 500);
  }
}
