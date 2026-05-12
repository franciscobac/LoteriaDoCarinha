using System.ComponentModel.DataAnnotations;

namespace Loteria.Core.Entities
{
    public class TipoLoteria
    {
        [ Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int QuantidadeNumeros { get; set; }

        [Required]
        public int MinNumero { get; set; } = 1;

        [Required]
        public int MaxNumero { get; set; }

        //Relacionamentos
        public virtual ICollection<Concurso> Concursos { get; set; } = new List<Concurso>();
        public virtual ICollection<NumerosGerados> NumerosGerados { get; set; } = new List<NumerosGerados>();
    }
}