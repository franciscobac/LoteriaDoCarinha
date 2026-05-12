using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LoteriaAPI.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<EmailSendResult> EnviarCodigoConfirmacaoAsync(string email, string nome, string codigo)
    {
        if (string.IsNullOrWhiteSpace(_settings.SmtpServer) ||
            string.IsNullOrWhiteSpace(_settings.FromEmail) ||
            string.IsNullOrWhiteSpace(_settings.Username) ||
            string.IsNullOrWhiteSpace(_settings.Password) ||
            _settings.Port <= 0)
        {
            _logger.LogError("Configuração de e-mail inválida. Verifique a seção EmailSettings.");
            return EmailSendResult.Fail(
                "Não foi possível enviar o código por e-mail. As configurações de e-mail da aplicação estão incompletas.",
                "EmailSettings inválido ou incompleto.");
        }

        try
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Confirmação de cadastro - Loteria do Carinha";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.5;'>
                        <h2>Olá, {nome}!</h2>
                        <p>Seu cadastro na <strong>Loteria do Carinha</strong> está quase concluído.</p>
                        <p>Use o código abaixo para confirmar seu e-mail:</p>
                        <div style='font-size: 28px; font-weight: bold; letter-spacing: 6px; margin: 24px 0; color: #0d6efd;'>
                            {codigo}
                        </div>
                        <p>Este código expira em 15 minutos.</p>
                        <p>Se você não solicitou este cadastro, ignore este e-mail.</p>
                    </div>",
                TextBody = $"Olá, {nome}! Seu código de confirmação é {codigo}. Ele expira em 15 minutos."
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            var secureSocketOption = _settings.Port == 465
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;

            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, secureSocketOption);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Código de confirmação enviado com sucesso para {Email}", email);
            return EmailSendResult.Ok("Código enviado para seu e-mail. Verifique sua caixa de entrada.");
        }
        catch (AuthenticationException ex) when (ex.Message.Contains("Application-specific password required", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogError(ex, "Falha de autenticação SMTP do Gmail para {EmailRemetente}", _settings.FromEmail);
            return EmailSendResult.Fail(
                "Não foi possível enviar o código por e-mail porque a conta Gmail do remetente precisa de senha de app.",
                "Gmail SMTP exige 2FA e senha de aplicativo para autenticação.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail de confirmação para {Email}", email);
            return EmailSendResult.Fail(
                "Não foi possível enviar o código por e-mail. Verifique as configurações SMTP e tente novamente.",
                ex.Message);
        }
    }
}
