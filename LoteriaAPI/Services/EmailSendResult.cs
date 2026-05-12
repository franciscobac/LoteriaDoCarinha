namespace LoteriaAPI.Services;

public class EmailSendResult
{
    public bool Success { get; init; }
    public string UserMessage { get; init; } = string.Empty;
    public string? TechnicalMessage { get; init; }

    public static EmailSendResult Ok(string userMessage) => new()
    {
        Success = true,
        UserMessage = userMessage
    };

    public static EmailSendResult Fail(string userMessage, string? technicalMessage = null) => new()
    {
        Success = false,
        UserMessage = userMessage,
        TechnicalMessage = technicalMessage
    };
}