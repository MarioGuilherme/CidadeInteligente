using CidadeInteligente.Domain.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

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
}