import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NumerosService } from '../../services/numeros.service';
import { LOTERIAS_UI, obterLoteriaPorNome } from '../../data/loterias.data';

@Component({
  selector: 'app-historico',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './historico.html',
  styleUrls: ['./historico.css']
})
export class Historico implements OnInit {
  historico: any[] = [];
  busca = '';
  loteriaFiltro = 'todas';
  loterias = LOTERIAS_UI;
  carregando = false;
  erro: string | null = null;

  constructor(
    private numerosService: NumerosService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  get historicoFiltrado(): any[] {
    return this.historico.filter((item) => {
      const loteriaValida = this.loteriaFiltro === 'todas' || item.loteriaNome === this.loteriaFiltro;
      const buscaNormalizada = this.busca.trim().toLowerCase();

      if (!buscaNormalizada) {
        return loteriaValida;
      }

      const numerosTexto = Array.isArray(item.numeros) ? item.numeros.join(' ') : '';
      const texto = `${item.loteriaNome} ${item.observacao ?? ''} ${numerosTexto}`.toLowerCase();
      return loteriaValida && texto.includes(buscaNormalizada);
    });
  }

  ngOnInit() {
    this.carregarHistorico();
  }

  getLoteriaInfo(nome: string) {
    return obterLoteriaPorNome(nome);
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
          alert(err.error?.mensagem || 'Erro ao excluir');
        }
      });
    }
  }
}
