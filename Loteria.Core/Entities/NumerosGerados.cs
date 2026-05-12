using System.ComponentModel.DataAnnotations;

namespace Loteria.Core.Entities
{
    public class NumerosGerados
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }  

        [Required]
        public int TipoLoteriaId { get; set; }

        [Required]
        public string Numeros { get; set; } = string.Empty;

        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;

        public TimeSpan HoraGeracao { get; set; } = DateTime.UtcNow.TimeOfDay;

        // Propriedade adicional útil
        public string? Observacao { get; set; } // Usuário pode adicionar um comentário

        // Relacionamentos
        public virtual Usuario? Usuario { get; set; }
        public virtual TipoLoteria? TipoLoteria { get; set; }
    }
}