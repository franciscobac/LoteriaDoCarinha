namespace LoteriaAPI.Services;

public interface IEmailService
{
    Task<EmailSendResult> EnviarCodigoConfirmacaoAsync(string email, string nome, string codigo);
}
