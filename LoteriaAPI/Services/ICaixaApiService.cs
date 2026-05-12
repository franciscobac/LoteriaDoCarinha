using Loteria.Core.DTOs;

namespace LoteriaAPI.Services;

public interface ICaixaApiService
{
    Task<CaixaResponseDto?> BuscarConcursoAsync(string tipoLoteria, int numeroConcurso);
    Task<CaixaResponseDto?> BuscarUltimoConcursoAsync(string tipoLoteria);
    bool IsDadosValidos(CaixaResponseDto? dados);
}