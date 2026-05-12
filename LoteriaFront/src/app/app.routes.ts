import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/consultar', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./pages/login/login').then(m => m.Login) },
  { path: 'registro', loadComponent: () => import('./pages/registro/registro').then(m => m.Registro) },
  { path: 'consultar', loadComponent: () => import('./pages/consultar-concurso/consultar-concurso').then(m => m.ConsultarConcurso) },
  { path: 'gerar', canActivate: [authGuard], loadComponent: () => import('./pages/gerar-numeros/gerar-numeros').then(m => m.GerarNumeros) },
  { path: 'historico', canActivate: [authGuard], loadComponent: () => import('./pages/historico/historico').then(m => m.Historico) },
  { path: '**', redirectTo: '/consultar' }
];
