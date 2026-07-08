using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, IEmailService emailService, IHttpContextAccessor httpContextAccessor) : IRequestHandler<SendEmailRecoverCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailService _emailService = emailService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit?> Handle(SendEmailRecoverCommand request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByEmailSpecification = UserSpecifications.GetByEmailAndExceptUserId(request.Email).Build();
        User? user = await _unitOfWork.Users.GetBySpecAsync(getUserByEmailSpecification, cancellationToken);
        if (user is null) return Unit.Value;

        await _unitOfWork.ExecuteInTransactionAsync(user.SaveNewTokenToRecoverPassword, cancellationToken: cancellationToken);
        string rawHtmlBody = await File.ReadAllTextAsync("./Views/Auth/BodyEmail.html", cancellationToken);

        StringBuilder stringBuilder = new(rawHtmlBody);
        stringBuilder = stringBuilder
            .Replace("{{ USERNAME }}", user.Name)
            .Replace("{{ URL }}", $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}")
            .Replace("{{ TOKEN }}", user.TokenRecoverPassword);

        await _emailService.SendEmailAsync(user.Email, "Redefinição de Senha", stringBuilder.ToString());

        return Unit.Value;
    }
}
