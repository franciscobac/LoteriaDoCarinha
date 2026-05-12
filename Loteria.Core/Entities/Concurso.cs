using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Loteria.Core.Entities
{
    public class Concurso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TipoLoteriaId { get; set; }

        [Required]
        public int NumeroConcurso { get; set; }

        [Required]
        public DateTime DataSorteio { get; set; }

        [Required]
        public string NumerosSorteados { get; set; } = string.Empty;

        public bool Acumulou { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorAcumuladoProximo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ValorArrecadado { get; set; }

        public string? LocalSorteio { get; set; }

        public string? MunicipioUFSorteio { get; set; }

        // Para guardar as faixas de premiação
        public string? Premiacoes { get; set; } // Armazenar como JSON string

        public DateTime DataConsulta { get; set; } = DateTime.UtcNow; // Para controle de cache

        // Relacionamento
        [JsonIgnore]
        public virtual TipoLoteria? TipoLoteria { get; set; }
    }
}