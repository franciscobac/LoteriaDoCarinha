using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Loteria.Core.Entities;
using Loteria.Core.DTOs;
using LoteriaAPI.Data;
using LoteriaAPI.Services;

namespace LoteriaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AppDbContext context,
        IConfiguration configuration,
        IEmailService emailService,
        ILogger<AuthController> logger)
    {
        _context = context;
        _configuration = configuration;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarRequestDto model)
    {
        var usuarioExistente = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (usuarioExistente?.EmailConfirmado == true)
            return BadRequest(new { mensagem = "Email já cadastrado" });

        var codigo = new Random().Next(100000, 999999).ToString();

        Usuario usuario;
        if (usuarioExistente is not null)
        {
            usuario = usuarioExistente;
            usuario.Nome = model.Nome;
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(model.Senha);
            usuario.CodigoConfirmacao = codigo;
            usuario.CodigoConfirmacaoExpiracao = DateTime.UtcNow.AddMinutes(15);
            usuario.EmailConfirmado = false;
            usuario.EmailConfirmadoEm = null;
        }
        else
        {
            usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(model.Senha),
                DataCadastro = DateTime.UtcNow,
                EmailConfirmado = false,
                CodigoConfirmacao = codigo,
                CodigoConfirmacaoExpiracao = DateTime.UtcNow.AddMinutes(15)
            };

            _context.Usuarios.Add(usuario);
        }

        await _context.SaveChangesAsync();

        var emailResult = await _emailService.EnviarCodigoConfirmacaoAsync(model.Email, model.Nome, codigo);

        if (!emailResult.Success)
        {
            _logger.LogWarning("Cadastro criado/atualizado, mas o envio do e-mail falhou para {Email}. Motivo: {Motivo}", model.Email, emailResult.TechnicalMessage);
            return StatusCode(500, new
            {
                mensagem = emailResult.UserMessage,
                detalheTecnico = emailResult.TechnicalMessage,
                email = model.Email
            });
        }

        return Ok(new { 
            mensagem = emailResult.UserMessage,
            email = model.Email 
        });
    }

    [HttpPost("confirmar-email")]
    public async Task<IActionResult> ConfirmarEmail([FromBody] ConfirmarEmailRequestDto model)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == model.Email && !u.EmailConfirmado);

        if (usuario == null)
            return BadRequest(new { mensagem = "Usuário não encontrado ou já confirmado" });

        if (usuario.CodigoConfirmacao != model.Codigo)
            return BadRequest(new { mensagem = "Código inválido" });

        if (usuario.CodigoConfirmacaoExpiracao < DateTime.UtcNow)
            return BadRequest(new { mensagem = "Código expirado. Solicite um novo." });

        usuario.EmailConfirmado = true;
        usuario.EmailConfirmadoEm = DateTime.UtcNow;
        usuario.CodigoConfirmacao = null;
        usuario.CodigoConfirmacaoExpiracao = null;

        await _context.SaveChangesAsync();

        var token = GerarTokenJwt(usuario);

        return Ok(new
        {
            id = usuario.Id,
            nome = usuario.Nome,
            email = usuario.Email,
            dataCadastro = usuario.DataCadastro,
            token = token,
            mensagem = "Email confirmado com sucesso!"
        });
    }

    [HttpPost("reenviar-codigo")]
    public async Task<IActionResult> ReenviarCodigo([FromBody] ReenviarCodigoRequestDto model)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == model.Email && !u.EmailConfirmado);

        if (usuario == null)
            return BadRequest(new { mensagem = "Usuário não encontrado ou já confirmado" });

        var novoCodigo = new Random().Next(100000, 999999).ToString();
        
        usuario.CodigoConfirmacao = novoCodigo;
        usuario.CodigoConfirmacaoExpiracao = DateTime.UtcNow.AddMinutes(15);
        
        await _context.SaveChangesAsync();

        var emailResult = await _emailService.EnviarCodigoConfirmacaoAsync(model.Email, usuario.Nome, novoCodigo);
        
        if (!emailResult.Success)
            return StatusCode(500, new
            {
                mensagem = emailResult.UserMessage,
                detalheTecnico = emailResult.TechnicalMessage,
                email = model.Email
            });

        return Ok(new { mensagem = "Novo código enviado para seu e-mail" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto model)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
        
        if (usuario == null)
            return Unauthorized(new { mensagem = "Email ou senha inválidos" });

        if (!usuario.EmailConfirmado)
            return Unauthorized(new
            {
                mensagem = "Por favor, confirme seu e-mail antes de fazer login",
                requerConfirmacao = true,
                email = usuario.Email
            });

        if (!BCrypt.Net.BCrypt.Verify(model.Senha, usuario.SenhaHash))
            return Unauthorized(new { mensagem = "Email ou senha inválidos" });

        var token = GerarTokenJwt(usuario);

        return Ok(new
        {
            id = usuario.Id,
            nome = usuario.Nome,
            email = usuario.Email,
            dataCadastro = usuario.DataCadastro,
            token = token
        });
    }

    private string GerarTokenJwt(Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? throw new Exception("JWT SecretKey not configured"));
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpirationHours"] ?? "24")),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
