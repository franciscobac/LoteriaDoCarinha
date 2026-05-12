namespace Loteria.Core.DTOs;

public class ConcursoResponseDto
{
    public int Id { get; set; }
    public int NumeroConcurso { get; set; }
    public DateTime DataSorteio { get; set; }
    public List<int> NumerosSorteados { get; set; }
    public bool Acumulou { get; set; }
    public decimal? ValorAcumuladoProximo { get; set; }
    public decimal? ValorArrecadado { get; set; }
    public string? LocalSorteio { get; set; }
    public string? MunicipioUFSorteio { get; set; }
    public List<PremioFaixaDto>? Premiacoes { get; set; }
}

public class PremioFaixaDto
{
    public string? DescricaoFaixa { get; set; }
    public int Faixa { get; set; }
    public int NumeroGanhadores { get; set; }
    public decimal ValorPremio { get; set; }
}