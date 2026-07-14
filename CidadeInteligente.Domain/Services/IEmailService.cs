namespace CidadeInteligente.Domain.Services;

public interface IEmailService
{
    Task SendEmailAsync(string recipient, string subject, string htmlContent);
    Task SendRecoverPasswordEmailAsync(string to, string userName, string baseUrl, string token);
}
