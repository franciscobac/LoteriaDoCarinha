import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NumerosService } from '../../services/numeros.service';

@Component({
  selector: 'app-gerar-numeros',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './gerar-numeros.html',
  styleUrls: ['./gerar-numeros.css']
})
export class GerarNumeros {
  tipoLoteriaId = 1;
  observacao = '';
  numerosGerados: number[] | null = null;
  carregando = false;
  erro: string | null = null;

  constructor(
    private numerosService: NumerosService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  gerar() {
    // Resetar estados
    this.carregando = true;
    this.numerosGerados = null;
    this.erro = null;
    
    // Forçar atualização da UI
    this.cdr.detectChanges();
    
    const dados = {
      tipoLoteriaId: this.tipoLoteriaId,
      observacao: this.observacao
    };
    
    this.numerosService.gerarNumeros(dados).subscribe({
      next: (res: any) => {
        this.numerosGerados = res.numeros;
        this.carregando = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Erro:', err);
        if (err.status === 401) {
          this.erro = 'Sua sessão expirou. Faça login novamente.';
          this.router.navigate(['/login']);
        } else {
          this.erro = err.error?.mensagem || 'Erro ao gerar números';
        }
        this.carregando = false;
        this.cdr.detectChanges();
      }
    });
  }
}
