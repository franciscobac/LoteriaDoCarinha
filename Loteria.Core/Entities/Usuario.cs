using System.ComponentModel.DataAnnotations;

namespace Loteria.Core.Entities;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string SenhaHash { get; set; } = string.Empty;
    
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    
    public bool EmailConfirmado { get; set; } = false;
    
    public DateTime? EmailConfirmadoEm { get; set; }
    
    public string? CodigoConfirmacao { get; set; }
    
    public DateTime? CodigoConfirmacaoExpiracao { get; set; }
    
    public virtual ICollection<NumerosGerados> NumerosGerados { get; set; } = new List<NumerosGerados>();
}
