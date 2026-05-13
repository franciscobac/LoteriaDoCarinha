export interface LoteriaUiOption {
  id: number;
  slug: string;
  nome: string;
  nomeExibicao: string;
  simbolo: string;
  descricao: string;
  faixa: string;
  quantidadeNumeros: number;
  cor: string;
}

export const LOTERIAS_UI: LoteriaUiOption[] = [
  {
    id: 1,
    slug: 'megasena',
    nome: 'MEGA_SENA',
    nomeExibicao: 'Mega-Sena',
    simbolo: '🍀',
    descricao: 'A loteria mais famosa do Brasil',
    faixa: '6 números de 1 a 60',
    quantidadeNumeros: 6,
    cor: '#0ea5a4'
  },
  {
    id: 2,
    slug: 'lotofacil',
    nome: 'LOTOFACIL',
    nomeExibicao: 'Lotofácil',
    simbolo: '⭐',
    descricao: 'Mais chances com 15 dezenas',
    faixa: '15 números de 1 a 25',
    quantidadeNumeros: 15,
    cor: '#a855f7'
  },
  {
    id: 3,
    slug: 'quina',
    nome: 'QUINA',
    nomeExibicao: 'Quina',
    simbolo: '🎯',
    descricao: 'Sorteios frequentes e dinâmicos',
    faixa: '5 números de 1 a 80',
    quantidadeNumeros: 5,
    cor: '#2563eb'
  },
  {
    id: 4,
    slug: 'lotomania',
    nome: 'LOTOMANIA',
    nomeExibicao: 'Lotomania',
    simbolo: '🔥',
    descricao: 'Modalidade com 50 dezenas',
    faixa: '50 números de 1 a 100',
    quantidadeNumeros: 50,
    cor: '#f97316'
  },
  {
    id: 5,
    slug: 'duplasena',
    nome: 'DUPLA_SENA',
    nomeExibicao: 'Dupla Sena',
    simbolo: '💎',
    descricao: 'Duas chances no mesmo concurso',
    faixa: '6 números de 1 a 50',
    quantidadeNumeros: 6,
    cor: '#ec4899'
  },
  {
    id: 6,
    slug: 'timemania',
    nome: 'TIMEMANIA',
    nomeExibicao: 'Timemania',
    simbolo: '⚽',
    descricao: 'Para quem ama futebol e sorte',
    faixa: '10 números de 1 a 80',
    quantidadeNumeros: 10,
    cor: '#84cc16'
  },
  {
    id: 7,
    slug: 'diadesorte',
    nome: 'DIADESORTE',
    nomeExibicao: 'Dia de Sorte',
    simbolo: '🌞',
    descricao: 'Números e mês da sorte',
    faixa: '7 números de 1 a 31',
    quantidadeNumeros: 7,
    cor: '#f59e0b'
  }
];

export function obterLoteriaPorId(id: number): LoteriaUiOption {
  return LOTERIAS_UI.find((item) => item.id === id) ?? LOTERIAS_UI[0];
}

export function obterLoteriaPorSlug(slug: string): LoteriaUiOption {
  return LOTERIAS_UI.find((item) => item.slug === slug) ?? LOTERIAS_UI[0];
}

export function obterLoteriaPorNome(nome: string): LoteriaUiOption {
  const normalizado = nome.toUpperCase().replaceAll('-', '_').replaceAll(' ', '_');
  return LOTERIAS_UI.find((item) => item.nome === normalizado) ?? LOTERIAS_UI[0];
}