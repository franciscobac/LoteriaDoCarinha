export interface ConcursoResponse {
  concurso: number;
  dataSorteio: string;
  numerosSorteados: number[];
  acumulou: boolean;
  valorAcumuladoProximo: number;
  valorArrecadado: number;
  localSorteio: string;
  municipioUFSorteio: string;
  premiacoes?: PremioFaixa[];
}

export interface PremioFaixa {
  faixa: number;
  descricao: string;
  ganhadores: number;
  premio: number;
}

export interface UltimoConcursoResponse {
  concurso: number;
  dataApuracao: string;
  dataProximoConcurso: string;
  numerosSorteados: number[];
  acumulou: boolean;
  valorAcumuladoProximo: number;
  valorArrecadado: number;
  localSorteio: string;
  municipioUFSorteio: string;
  tipoJogo: string;
}