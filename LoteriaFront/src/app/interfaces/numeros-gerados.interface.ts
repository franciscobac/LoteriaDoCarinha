export interface NumerosGeradosRequest {
  tipoLoteriaId: number;
  observacao?: string;
}

export interface NumerosGeradosResponse {
  id: number;
  loteriaNome: string;
  numeros: number[];
  dataGeracao: Date;
  horaGeracao: string;
  observacao?: string;
}

export interface TipoLoteria {
  id: number;
  nome: string;
  quantidadeNumeros: number;
  minNumero: number;
  maxNumero: number;
}