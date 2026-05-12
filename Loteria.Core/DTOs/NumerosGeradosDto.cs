namespace Loteria.Core.DTOs;

public class NumerosGeradosRequestDto
{
    public int TipoLoteriaId { get; set; }
    public string? Observacao { get; set; }
}

public class NumerosGeradosResponseDto
{
    public int Id { get; set; }
    public string LoteriaNome { get; set; } = string.Empty;
    public List<int> Numeros { get; set; } = new();
    public DateTime DataGeracao { get; set; }
    public string HoraGeracao { get; set; } = string.Empty;
    public string? Observacao { get; set; }

}