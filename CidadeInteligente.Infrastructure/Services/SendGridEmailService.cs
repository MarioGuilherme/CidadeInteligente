using CidadeInteligente.Core.Services;
using SendGrid.Helpers.Mail;
using SendGrid;
using CidadeInteligente.Core.Exceptions;

namespace CidadeInteligente.Infrastructure.Services;

public class SendGridEmailService(string apiKey, string senderEmail) : IEmailService {
    private readonly string _apiKey = apiKey;
    private readonly string _senderEmail = senderEmail;

    public async Task SendEmailAsync(string recipient, string subject, string htmlContent) {
        SendGridClient client = new(this._apiKey);
        EmailAddress from = new(this._senderEmail);
        EmailAddress recipientEmailAddress = new(recipient);
        SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(from, recipientEmailAddress, subject, string.Empty, htmlContent);
        Response response = await client.SendEmailAsync(sendGridMessage);
        if (!response.IsSuccessStatusCode) throw new SendEmailException();
    }
}