import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConcursoService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  buscarConcurso(loteria: string, concurso: number) {
    return this.http.get(`${this.apiUrl}/Concursos/buscar?loteria=${loteria}&concurso=${concurso}`);
  }

  buscarUltimoConcurso(loteria: string) {
    return this.http.get(`${this.apiUrl}/Concursos/ultimo?loteria=${loteria}`);
  }
}
