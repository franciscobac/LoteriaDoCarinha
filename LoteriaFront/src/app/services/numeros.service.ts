import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NumerosService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  gerarNumeros(dados: any) {
    return this.http.post(`${this.apiUrl}/Numeros/gerar`, dados);
  }

  obterHistorico() {
    return this.http.get(`${this.apiUrl}/Numeros/historico`);
  }

  deletarRegistro(id: number) {
    return this.http.delete(`${this.apiUrl}/Numeros/${id}`);
  }
}
