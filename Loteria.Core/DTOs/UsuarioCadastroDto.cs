using System.ComponentModel.DataAnnotations;

namespace Loteria.Core.DTOs;

public class UsuarioCadastroDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve conter no mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;
}

public class UsuarioLoginDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;
}

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public string Token { get; set; } = string.Empty;
}