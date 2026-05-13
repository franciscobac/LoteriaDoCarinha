import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ConcursoService } from '../../services/concurso.service';
import { LOTERIAS_UI, obterLoteriaPorSlug } from '../../data/loterias.data';
import { ConcursoResponse, UltimoConcursoResponse } from '../../interfaces/concurso.interface';

@Component({
  selector: 'app-consultar',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './consultar-concurso.html',
  styleUrls: ['./consultar-concurso.css']
})
export class ConsultarConcurso {
  loteria = 'megasena';
  concursoNumero?: number;
  carregando = false;
  resultado: ConcursoResponse | null = null;
  erro: string | null = null;
  loterias = LOTERIAS_UI;

  constructor(
    private concursoService: ConcursoService,
    private cdr: ChangeDetectorRef
  ) {}

  get loteriaSelecionada() {
    return obterLoteriaPorSlug(this.loteria);
  }

  buscar() {
    if (!this.concursoNumero) {
      this.erro = 'Digite o número do concurso para pesquisar.';
      return;
    }

    this.carregando = true;
    this.resultado = null;
    this.erro = null;
    this.cdr.detectChanges();

    this.concursoService.buscarConcurso(this.loteria, this.concursoNumero)
      .subscribe({
        next: (res) => {
          this.resultado = res;
          this.carregando = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.erro = 'Concurso não encontrado para essa loteria.';
          this.carregando = false;
          this.cdr.detectChanges();
        }
      });
  }

  buscarUltimo() {
    this.carregando = true;
    this.resultado = null;
    this.erro = null;
    this.cdr.detectChanges();

    this.concursoService.buscarUltimoConcurso(this.loteria)
      .subscribe({
        next: (res: UltimoConcursoResponse) => {
          this.concursoNumero = res.concurso;

          // Busca os detalhes completos do concurso mais recente,
          // incluindo as faixas de premiação.
          this.concursoService.buscarConcurso(this.loteria, res.concurso)
            .subscribe({
              next: (concursoCompleto: ConcursoResponse) => {
                this.resultado = concursoCompleto;
                this.carregando = false;
                this.cdr.detectChanges();
              },
              error: () => {
                // Fallback para não deixar a tela vazia caso a busca detalhada falhe.
                this.resultado = {
                  concurso: res.concurso,
                  dataSorteio: res.dataApuracao,
                  numerosSorteados: res.numerosSorteados,
                  acumulou: res.acumulou,
                  valorAcumuladoProximo: res.valorAcumuladoProximo,
                  valorArrecadado: res.valorArrecadado,
                  localSorteio: res.localSorteio,
                  municipioUFSorteio: res.municipioUFSorteio,
                  premiacoes: []
                };
                this.carregando = false;
                this.cdr.detectChanges();
              }
            });
        },
        error: () => {
          this.erro = `Não foi possível buscar o último concurso da ${this.loteriaSelecionada.nomeExibicao}.`;
          this.carregando = false;
          this.cdr.detectChanges();
        }
      });
  }
}
