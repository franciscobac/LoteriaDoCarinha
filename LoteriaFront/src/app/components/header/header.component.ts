import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
      <div class="container">
        <a class="navbar-brand" routerLink="/">LoteriaApp</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <a class="nav-link" routerLink="/consultar" routerLinkActive="active">Consultar Concurso</a>
            </li>
            <li class="nav-item" *ngIf="isAutenticado">
              <a class="nav-link" routerLink="/gerar" routerLinkActive="active">Gerar Números</a>
            </li>
            <li class="nav-item" *ngIf="isAutenticado">
              <a class="nav-link" routerLink="/historico" routerLinkActive="active">Meu Histórico</a>
            </li>
          </ul>
          <ul class="navbar-nav">
            <li class="nav-item" *ngIf="!isAutenticado">
              <a class="nav-link" routerLink="/login" routerLinkActive="active">Login</a>
            </li>
            <li class="nav-item" *ngIf="!isAutenticado">
              <a class="nav-link" routerLink="/registro" routerLinkActive="active">Registrar</a>
            </li>
            <li class="nav-item" *ngIf="isAutenticado">
              <span class="nav-link">Olá, {{ usuario?.nome }}</span>
            </li>
            <li class="nav-item" *ngIf="isAutenticado">
              <button class="btn btn-outline-danger btn-sm ms-2" (click)="logout()">Sair</button>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  `
})
export class HeaderComponent {
  constructor(private authService: AuthService) {}

  get isAutenticado(): boolean {
    return this.authService.isAutenticado();
  }

  get usuario(): any {
    return this.authService.getUsuarioLogado();
  }

  logout(): void {
    this.authService.logout();
  }
}
