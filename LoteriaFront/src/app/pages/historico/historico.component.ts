import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-historico',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card">
      <div class="card-header bg-secondary text-white">
        <h4 class="mb-0">Meu Histórico</h4>
      </div>
      <div class="card-body">
        <div class="alert alert-info">
          📋 Exemplo de histórico (dados simulados)
        </div>
        
        <div class="card mb-3">
          <div class="card-body">
            <div class="d-flex justify-content-between">
              <div>
                <h5>Mega-Sena</h5>
                <p class="text-muted">Gerado em: 09/05/2026 às 22:30</p>
              </div>
              <button class="btn btn-danger btn-sm">Excluir</button>
            </div>
            <div class="d-flex flex-wrap gap-2 mt-2">
              <span *ngFor="let num of [5, 12, 23, 34, 45, 56]" class="badge bg-info p-2 fs-6">
                {{ num }}
              </span>
            </div>
          </div>
        </div>
        
        <div class="card mb-3">
          <div class="card-body">
            <div class="d-flex justify-content-between">
              <div>
                <h5>Lotofácil</h5>
                <p class="text-muted">Gerado em: 08/05/2026 às 15:20</p>
              </div>
              <button class="btn btn-danger btn-sm">Excluir</button>
            </div>
            <div class="d-flex flex-wrap gap-2 mt-2">
              <span *ngFor="let num of [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15]" class="badge bg-info p-2 fs-6">
                {{ num }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class HistoricoComponent {}
