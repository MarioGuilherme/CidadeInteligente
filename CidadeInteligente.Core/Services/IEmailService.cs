namespace CidadeInteligente.Core.Services;

public interface IEmailService {
    Task SendEmailAsync(string recipient, string subject, string htmlContent);
}