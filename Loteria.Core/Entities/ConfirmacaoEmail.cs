using System.ComponentModel.DataAnnotations;

namespace Loteria.Core.Entities;

public class ConfirmacaoEmail
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UsuarioId { get; set; }
    
    [Required]
    [MaxLength(6)]
    public string Codigo { get; set; } = string.Empty;
    
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    
    public DateTime Expiracao { get; set; }
    
    public bool Usado { get; set; } = false;
    
    public virtual Usuario? Usuario { get; set; }
}
