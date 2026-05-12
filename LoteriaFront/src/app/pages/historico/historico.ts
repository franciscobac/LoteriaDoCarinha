import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { NumerosService } from '../../services/numeros.service';

@Component({
  selector: 'app-historico',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './historico.html',
  styleUrls: ['./historico.css']
})
export class Historico implements OnInit {
  historico: any[] = [];
  carregando = false;
  erro: string | null = null;

  constructor(
    private numerosService: NumerosService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.carregarHistorico();
  }

  carregarHistorico() {
    this.carregando = true;
    this.erro = null;
    this.cdr.detectChanges();
    
    this.numerosService.obterHistorico().subscribe({
      next: (res: any) => {
        this.historico = res;
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        if (err.status === 401) {
          this.erro = 'Sua sessão expirou. Faça login novamente.';
          this.router.navigate(['/login']);
        } else {
          this.erro = err.error?.mensagem || 'Erro ao carregar histórico';
        }
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }

  deletar(id: number) {
    if (confirm('Deseja excluir este registro?')) {
      this.numerosService.deletarRegistro(id).subscribe({
        next: () => {
          this.carregarHistorico();
        },
        error: (err) => {
          console.error(err);
          alert(err.error?.mensagem || 'Erro ao excluir');
        }
      });
    }
  }
}
