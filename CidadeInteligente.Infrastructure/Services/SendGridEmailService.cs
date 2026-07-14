using CidadeInteligente.Domain.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Reflection;
using System.Text;

namespace CidadeInteligente.Infrastructure.Services;

public class SendGridEmailService(string apiKey, string senderEmail) : IEmailService
{
    private readonly string _apiKey = apiKey;
    private readonly string _senderEmail = senderEmail;

    public async Task SendEmailAsync(string recipient, string subject, string htmlContent)
    {
        SendGridClient client = new(_apiKey);
        EmailAddress from = new(_senderEmail);
        EmailAddress recipientEmailAddress = new(recipient);
        SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(from, recipientEmailAddress, subject, string.Empty, htmlContent);
        await client.SendEmailAsync(sendGridMessage);
    }

    public async Task SendRecoverPasswordEmailAsync(string to, string userName, string baseUrl, string token)
    {
        string template = await ReadTemplateAsync("RecoverPasswordEmail.html");

        string body = new StringBuilder(template)
            .Replace("{{ USERNAME }}", userName)
            .Replace("{{ URL }}", baseUrl)
            .Replace("{{ TOKEN }}", token)
            .ToString();

        await SendEmailAsync(to, "Redefinição de Senha", body);
    }

    private static async Task<string> ReadTemplateAsync(string fileName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string resourceName = $"CidadeInteligente.Infrastructure.Templates.{fileName}";

        await using Stream stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Template '{resourceName}' not found as an embedded resource.");

        using StreamReader reader = new(stream);
        return await reader.ReadToEndAsync();
    }
}
