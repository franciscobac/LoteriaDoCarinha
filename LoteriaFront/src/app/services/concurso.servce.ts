import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConcursoResponse, UltimoConcursoResponse } from '../interfaces/concurso.interface';

@Injectable({
  providedIn: 'root'
})
export class ConcursoService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5084/api/Concursos';

  buscarConcurso(loteria: string, concurso: number): Observable<ConcursoResponse> {
    return this.http.get<ConcursoResponse>(`${this.apiUrl}/buscar?loteria=${loteria}&concurso=${concurso}`);
  }

  buscarUltimoConcurso(loteria: string): Observable<UltimoConcursoResponse> {
    return this.http.get<UltimoConcursoResponse>(`${this.apiUrl}/ultimo?loteria=${loteria}`);
  }
}