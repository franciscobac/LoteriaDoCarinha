using System.ComponentModel.DataAnnotations;

namespace Loteria.Core.DTOs;

public class RegistrarRequestDto
{
    [Required]
    public string Nome { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;
}

public class ConfirmarEmailRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Codigo { get; set; } = string.Empty;
}

public class ReenviarCodigoRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
