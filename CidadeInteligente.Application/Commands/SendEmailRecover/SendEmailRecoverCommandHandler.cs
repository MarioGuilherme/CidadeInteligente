using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, IHttpContextAccessor httpContextAccessor) : IRequestHandler<SendEmailRecoverCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailService _emailService = emailService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit> Handle(SendEmailRecoverCommand request, CancellationToken cancellationToken) {
        User? user = await this._unitOfWork.Users.GetByEmailAsync(request.Email, true);

        if (user is null) return Unit.Value;

        user.SaveNewTokenToRecoverPassword();

        string rawHtmlBody = await File.ReadAllTextAsync("./Views/Auth/BodyEmail.html", cancellationToken);
        
        StringBuilder stringBuilder = new(rawHtmlBody);

        stringBuilder = stringBuilder
            .Replace("{{ USERNAME }}", user.Name)
            .Replace("{{ URL }}", $"{this._httpContextAccessor.HttpContext!.Request.Scheme}://{this._httpContextAccessor.HttpContext.Request.Host}")
            .Replace("{{ TOKEN }}", user.TokenRecoverPassword);

        await Task.WhenAll(
            this._unitOfWork.CompleteAsync(),
            this._emailService.SendEmailAsync(user.Email, "Redefinição de Senha", stringBuilder.ToString())
        );

        return Unit.Value;
    }
}