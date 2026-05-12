import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isAuthenticated());
  private usuarioSubject = new BehaviorSubject<any>(this.getUsuario());

  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  usuario$ = this.usuarioSubject.asObservable();

  private isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  private getUsuario(): any {
    const usuario = localStorage.getItem('usuario');
    return usuario ? JSON.parse(usuario) : null;
  }

  updateAuthState() {
    this.isAuthenticatedSubject.next(this.isAuthenticated());
    this.usuarioSubject.next(this.getUsuario());
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuario');
    this.updateAuthState();
  }
}
