using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Loteria.Core.Entities;
using Loteria.Core.DTOs;
using LoteriaAPI.Data;
using LoteriaAPI.Services;
using System.Text.Json;

namespace LoteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConcursosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICaixaApiService _caixaApiService;
    private readonly ILogger<ConcursosController> _logger;

    public ConcursosController(
        AppDbContext context,
        ICaixaApiService caixaApiService,
        ILogger<ConcursosController> logger)
    {
        _context = context;
        _caixaApiService = caixaApiService;
        _logger = logger;
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarConcurso(
        [FromQuery] string loteria,
        [FromQuery] int concurso)
    {
        // Buscar na API da Caixa (o serviço já normaliza o nome)
        var dadosCaixa = await _caixaApiService.BuscarConcursoAsync(loteria, concurso);

        if (!_caixaApiService.IsDadosValidos(dadosCaixa))
            return NotFound($"Concurso {concurso} da loteria {loteria} não encontrado");

        // Retornar os dados formatados
        return Ok(new
        {
            concurso = dadosCaixa!.Numero,
            dataSorteio = dadosCaixa.DataApuracao,
            numerosSorteados = dadosCaixa.ListaDezenas.Select(int.Parse).ToList(),
            acumulou = dadosCaixa.Acumulado,
            valorAcumuladoProximo = dadosCaixa.ValorAcumuladoProximoConcurso,
            valorArrecadado = dadosCaixa.ValorArrecadado,
            localSorteio = dadosCaixa.LocalSorteio,
            municipioUFSorteio = dadosCaixa.NomeMunicipioUFSorteio,
            premiacoes = dadosCaixa.ListaRateioPremio?.Select(p => new
            {
                faixa = p.Faixa,
                descricao = p.DescricaoFaixa,
                ganhadores = p.NumeroDeGanhadores,
                premio = p.ValorPremio
            })
        });
    }

    [HttpGet("ultimo")]
    public async Task<IActionResult> BuscarUltimoConcurso([FromQuery] string loteria)
    {
        // Buscar na API da Caixa (o serviço já normaliza o nome)
        var dadosCaixa = await _caixaApiService.BuscarUltimoConcursoAsync(loteria);
        
        if (!_caixaApiService.IsDadosValidos(dadosCaixa))
            return NotFound($"Não foi possível buscar o último concurso da {loteria}");

        return Ok(new
        {
            concurso = dadosCaixa!.Numero,
            dataApuracao = dadosCaixa.DataApuracao,
            dataProximoConcurso = dadosCaixa.DataProximoConcurso,
            numerosSorteados = dadosCaixa.ListaDezenas.Select(int.Parse).ToList(),
            acumulou = dadosCaixa.Acumulado,
            valorAcumuladoProximo = dadosCaixa.ValorAcumuladoProximoConcurso,
            valorArrecadado = dadosCaixa.ValorArrecadado,
            localSorteio = dadosCaixa.LocalSorteio,
            municipioUFSorteio = dadosCaixa.NomeMunicipioUFSorteio,
            tipoJogo = dadosCaixa.TipoJogo
        });
    }
}