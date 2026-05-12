import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5084/api';
  
  // Evento para notificar mudanças no estado de autenticação
  authChange = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  registrar(usuario: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/registrar`, usuario);
  }

  confirmarEmail(dados: { email: string; codigo: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/confirmar-email`, dados);
  }

  reenviarCodigo(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/reenviar-codigo`, { email });
  }

  login(credenciais: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/login`, credenciais);
  }

  salvarSessao(resposta: any) {
    this.salvarToken(resposta.token);
    this.salvarUsuario({
      id: resposta.id,
      nome: resposta.nome,
      email: resposta.email,
      dataCadastro: resposta.dataCadastro
    });
  }

  salvarToken(token: string) {
    localStorage.setItem('token', token);
  }

  salvarUsuario(usuario: any) {
    localStorage.setItem('usuario', JSON.stringify(usuario));
    // Emitir evento de login
    this.authChange.emit(true);
  }

  logout() {
    this.limparSessao();
    this.router.navigate(['/login']);
  }

  limparSessao() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.authChange.emit(false);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAutenticado(): boolean {
    return !!this.getToken();
  }

  getUsuarioLogado(): any {
    const usuario = localStorage.getItem('usuario');
    return usuario ? JSON.parse(usuario) : null;
  }
}
