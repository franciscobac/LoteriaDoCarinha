using System.Text.Json.Serialization;

namespace Loteria.Core.DTOs;

public class CaixaResponseDto
{
    [JsonPropertyName("numero")]
    public int Numero { get; set; }
    
    [JsonPropertyName("dataApuracao")] 
    public string? DataApuracao { get; set; } 
    
    [JsonPropertyName("dataProximoConcurso")]
    public string? DataProximoConcurso { get; set; }

    [JsonPropertyName("listaDezenas")]
    public List<string> ListaDezenas { get; set; } = new();
    
    [JsonPropertyName("acumulado")] 
    public bool Acumulado { get; set; }
    
    [JsonPropertyName("valorAcumuladoProximoConcurso")]
    public decimal ValorAcumuladoProximoConcurso { get; set; }
    
    [JsonPropertyName("valorArrecadado")] public decimal ValorArrecadado { get; set; }
    
    [JsonPropertyName("localSorteio")] 
    public string? LocalSorteio { get; set; }
    
    [JsonPropertyName("nomeMunicipioUFSorteio")]
    public string? NomeMunicipioUFSorteio { get; set; }
    
    [JsonPropertyName("listaRateioPremio")]
    public List<RateioPremioDto>? ListaRateioPremio { get; set; }
    
    [JsonPropertyName("tipoJogo")] 
    public string? TipoJogo { get; set; }
}

public class RateioPremioDto
{
    [JsonPropertyName("descricaoFaixa")] 
    public string? DescricaoFaixa  { get; set; }
    
    [JsonPropertyName("faixa")] 
    public int Faixa { get; set; }
    
    [JsonPropertyName("numeroDeGanhadores")] 
    public int NumeroDeGanhadores { get; set; }
    
    [JsonPropertyName("valorPremio")] 
    public decimal ValorPremio { get; set; }
}