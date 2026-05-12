using System.Text.Json;
using Loteria.Core.DTOs;

namespace LoteriaAPI.Services;

public class CaixaApiService : ICaixaApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CaixaApiService> _logger;

    public CaixaApiService(HttpClient httpClient, ILogger<CaixaApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    
    // Função auxiliar para normalizar o nome da loteria para o formato da API
    private string NormalizarNomeLoteria(string tipoLoteria)
    {
        return tipoLoteria
            .ToLower()
            .Replace("_", "")
            .Replace("-", "");
    }
    
    public async Task<CaixaResponseDto?> BuscarConcursoAsync(string tipoLoteria, int numeroConcurso)
    {
        try
        {
            var nomeNormalizado = NormalizarNomeLoteria(tipoLoteria);
            var url = $"https://servicebus2.caixa.gov.br/portaldeloterias/api/{nomeNormalizado}/{numeroConcurso}";
            
            _logger.LogInformation("Buscando URL: {Url}", url);
            
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) 
                return null;
            
            var json = await response.Content.ReadAsStringAsync();
            var dados = JsonSerializer.Deserialize<CaixaResponseDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return dados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar concurso {TipoLoteria}/{Numero}", tipoLoteria, numeroConcurso);
            return null;
        } 
    }

    public async Task<CaixaResponseDto?> BuscarUltimoConcursoAsync(string tipoLoteria)
    {
        try
        {
            var nomeNormalizado = NormalizarNomeLoteria(tipoLoteria);
            var url = $"https://servicebus2.caixa.gov.br/portaldeloterias/api/{nomeNormalizado}";
            
            _logger.LogInformation("Buscando URL: {Url}", url);
            
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) 
                return null;
            
            var json = await response.Content.ReadAsStringAsync();
            var dados = JsonSerializer.Deserialize<CaixaResponseDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            
            return dados;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar último concurso da {TipoLoteria}", tipoLoteria);
            return null;
        }
    }

    public bool IsDadosValidos(CaixaResponseDto? dados)
    {
        return dados != null && dados.ListaDezenas != null && dados.ListaDezenas.Any();
    }
}