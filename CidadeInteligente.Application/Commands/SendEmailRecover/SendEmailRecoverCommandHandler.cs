using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;
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
        //Specification<User> spec = SpecificationBuilder<User>.Create()
        //    .Where(u => u.Email == request.Email)
        //    .WithProjection(u => new(u.UserId, u.Password, u.Role))
        //    .NoTracking()
        //    .Build();

        //User? user = await _unitOfWork.Users.GetProjectionBySpecAsync(spec);

        Specification<User> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.Email == request.Email)
            .AsEditable()
            .Build();

        User? user = await _unitOfWork.Users.GetBySpecAsync(spec);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserWithEmailNotFound);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        user.SaveNewTokenToRecoverPassword();

        string rawHtmlBody = await File.ReadAllTextAsync("./Views/Auth/BodyEmail.html", cancellationToken);

        StringBuilder stringBuilder = new(rawHtmlBody);
        stringBuilder = stringBuilder
            .Replace("{{ USERNAME }}", user.Name)
            .Replace("{{ URL }}", $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}")
            .Replace("{{ TOKEN }}", user.TokenRecoverPassword);

        await _emailService.SendEmailAsync(user.Email, "Redefinição de Senha", stringBuilder.ToString());
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
