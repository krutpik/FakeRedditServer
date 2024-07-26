using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace FakeReddit.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    private readonly HttpClient client = new HttpClient();

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
        ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var values = new Dictionary<string, string>
        {
            { "format", "json" },
            { "api_key", Options.UnisenderKey },
            { "email", toEmail },
            { "sender_name", "krutpik" },
            { "sender_email", "sarafor670@gmail.com" },
            { "subject", subject },
            { "body", message },
            { "list_id", "1" },
        };
        var content = new FormUrlEncodedContent(values);
        await client.PostAsync("https://api.unisender.com/ru/api/sendEmail", content);
    }
}