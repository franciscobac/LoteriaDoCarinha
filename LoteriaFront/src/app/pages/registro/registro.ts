import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './registro.html',
  styleUrls: ['./registro.css']
})
export class Registro {
  nome = '';
  email = '';
  senha = '';
  codigo = '';
  erro = '';
  mensagem = '';
  carregando = false;
  cadastroPendente = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  onSubmit() {
    this.carregando = true;
    this.erro = '';
    this.mensagem = '';
    this.cdr.detectChanges();
    
    const dados = {
      nome: this.nome,
      email: this.email,
      senha: this.senha
    };
    
    this.authService.registrar(dados).subscribe({
      next: (res: any) => {
        this.cadastroPendente = true;
        this.email = res.email || this.email;
        this.codigo = '';
        this.mensagem = res.mensagem || 'Código enviado para seu e-mail.';
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.erro = err.error?.mensagem || 'Erro ao registrar';
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }

  confirmarCodigo() {
    if (!this.email || !this.codigo) {
      this.erro = 'Informe o código recebido por e-mail.';
      return;
    }

    this.carregando = true;
    this.erro = '';
    this.mensagem = '';
    this.cdr.detectChanges();

    this.authService.confirmarEmail({ email: this.email, codigo: this.codigo }).subscribe({
      next: (res: any) => {
        this.authService.salvarSessao(res);
        this.router.navigate(['/consultar']);
      },
      error: (err) => {
        console.error(err);
        this.erro = err.error?.mensagem || 'Erro ao confirmar cadastro';
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }

  reenviarCodigo() {
    if (!this.email) {
      this.erro = 'Informe o e-mail cadastrado.';
      return;
    }

    this.carregando = true;
    this.erro = '';
    this.mensagem = '';
    this.cdr.detectChanges();

    this.authService.reenviarCodigo(this.email).subscribe({
      next: (res: any) => {
        this.mensagem = res.mensagem || 'Novo código enviado.';
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.erro = err.error?.mensagem || 'Erro ao reenviar código';
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }
}
