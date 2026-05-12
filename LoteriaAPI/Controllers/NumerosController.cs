using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Loteria.Core.Entities;
using Loteria.Core.DTOs;
using LoteriaAPI.Data;

namespace LoteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requer autenticação para todos os endpoints
public class NumerosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly Random _random;
    private readonly ILogger<NumerosController> _logger;

    public NumerosController(AppDbContext context, ILogger<NumerosController> logger)
    {
        _context = context;
        _logger = logger;
        _random = new Random();
    }

    private int ObterUsuarioId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    [HttpPost("gerar")]
    public async Task<IActionResult> GerarNumeros([FromBody] NumerosGeradosRequestDto request)
    {
        // Buscar a loteria
        var loteria = await _context.Loterias.FindAsync(request.TipoLoteriaId);
        if (loteria == null)
            return BadRequest(new { mensagem = "Loteria não encontrada" });

        // Gerar números aleatórios
        var numerosGerados = new HashSet<int>();
        while (numerosGerados.Count < loteria.QuantidadeNumeros)
        {
            var numero = _random.Next(loteria.MinNumero, loteria.MaxNumero + 1);
            numerosGerados.Add(numero);
        }

        var numerosOrdenados = numerosGerados.OrderBy(n => n).ToList();
        var numerosString = string.Join(",", numerosOrdenados);

        // Salvar no banco
        var usuarioId = ObterUsuarioId();
        var registros = new NumerosGerados
        {
            UsuarioId = usuarioId,
            TipoLoteriaId = loteria.Id,
            Numeros = numerosString,
            DataGeracao = DateTime.UtcNow,
            HoraGeracao = DateTime.UtcNow.TimeOfDay,
            Observacao = request.Observacao
        };

        _context.NumerosGerados.Add(registros);
        await _context.SaveChangesAsync();

        return Ok(new NumerosGeradosResponseDto
        {
            Id = registros.Id,
            LoteriaNome = loteria.Nome,
            Numeros = numerosOrdenados,
            DataGeracao = registros.DataGeracao,
            HoraGeracao = registros.HoraGeracao.ToString(@"hh\:mm\:ss"),
            Observacao = registros.Observacao
        });
    }

    [HttpGet("historico")]
    public async Task<IActionResult> ObterHistorico()
    {
        var usuarioId = ObterUsuarioId();
        
        var historicoDb = await _context.NumerosGerados
            .Include(n => n.TipoLoteria)
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.DataGeracao)
            .ThenByDescending(n => n.HoraGeracao)
            .ToListAsync();

        var historico = historicoDb
            .Select(n => new NumerosGeradosResponseDto
            {
                Id = n.Id,
                LoteriaNome = n.TipoLoteria?.Nome ?? "Desconhecida",
                Numeros = n.Numeros.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(),
                DataGeracao = n.DataGeracao,
                HoraGeracao = n.HoraGeracao.ToString(@"hh\:mm\:ss"),
                Observacao = n.Observacao
            })
            .ToList();

        return Ok(historico);
    }

    [HttpGet("estatisticas")]
    public async Task<IActionResult> ObterEstatisticas()
    {
        var usuarioId = ObterUsuarioId();
        
        var estatisticas = await _context.NumerosGerados
            .Include(n => n.TipoLoteria)
            .Where(n => n.UsuarioId == usuarioId)
            .GroupBy(n => n.TipoLoteria!.Nome)
            .Select(g => new
            {
                Loteria = g.Key,
                TotalGeracoes = g.Count(),
                UltimaGeracao = g.Max(n => n.DataGeracao)
            })
            .ToListAsync();

        return Ok(estatisticas);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarRegistro(int id)
    {
        var usuarioId = ObterUsuarioId();
        
        var registro = await _context.NumerosGerados
            .FirstOrDefaultAsync(n => n.Id == id && n.UsuarioId == usuarioId);
        
        if (registro == null)
            return NotFound(new { mensagem = "Registro não encontrado" });

        _context.NumerosGerados.Remove(registro);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Registro removido com sucesso" });
    }
}