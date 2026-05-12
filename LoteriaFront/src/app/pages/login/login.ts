import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  email = '';
  senha = '';
  codigo = '';
  erro = '';
  mensagem = '';
  carregando = false;
  pendenteConfirmacao = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  onSubmit() {
    this.carregando = true;
    this.erro = '';
    this.cdr.detectChanges(); // Forçar atualização da UI
    
    const dados = {
      email: this.email,
      senha: this.senha
    };
    
    this.authService.login(dados).subscribe({
      next: (res: any) => {
        this.authService.salvarSessao(res);
        this.router.navigate(['/consultar']);
      },
      error: (err) => {
        console.error(err);
        this.erro = err.error?.mensagem || 'Email ou senha inválidos';
        this.mensagem = '';
        this.pendenteConfirmacao = !!err.error?.requerConfirmacao;
        if (err.error?.email) {
          this.email = err.error.email;
        }
        this.carregando = false;
        this.cdr.detectChanges(); // Forçar atualização da UI com o erro
      }
    });
  }

  confirmarCadastro() {
    if (!this.email || !this.codigo) {
      this.erro = 'Informe o e-mail e o código recebido.';
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
        this.pendenteConfirmacao = true;
        this.mensagem = res.mensagem || 'Novo código enviado para seu e-mail';
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
