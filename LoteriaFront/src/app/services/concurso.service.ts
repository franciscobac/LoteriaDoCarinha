import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ConcursoResponse, UltimoConcursoResponse } from '../interfaces/concurso.interface';

@Injectable({
  providedIn: 'root'
})
export class ConcursoService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  buscarConcurso(loteria: string, concurso: number): Observable<ConcursoResponse> {
    return this.http.get<ConcursoResponse>(`${this.apiUrl}/Concursos/buscar?loteria=${loteria}&concurso=${concurso}`);
  }

  buscarUltimoConcurso(loteria: string): Observable<UltimoConcursoResponse> {
    return this.http.get<UltimoConcursoResponse>(`${this.apiUrl}/Concursos/ultimo?loteria=${loteria}`);
  }
}
