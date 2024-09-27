using Azure.Core;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, IHttpContextAccessor httpContextAccessor, IRazorViewRenderer razorViewRenderer) : IRequestHandler<SendEmailRecoverCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailService _emailService = emailService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IRazorViewRenderer _razorViewRenderer = razorViewRenderer;

    public async Task<Unit> Handle(SendEmailRecoverCommand request, CancellationToken cancellationToken) {
        User? user = await this._unitOfWork.Users.GetByEmailAsync(request.Email, true);

        if (user is null) return Unit.Value;

        user.SaveNewTokenToRecoverPassword();

        string htmlContent = await this._razorViewRenderer.RenderViewToStringAsync<UserDataChangePassword>(
            "./Auth/BodyEmail",
            new(user.Name, user.TokenRecoverPassword!),
            $"{this._httpContextAccessor.HttpContext!.Request.Scheme}://{this._httpContextAccessor.HttpContext.Request.Host}"
        );

        await Task.WhenAll(
            this._unitOfWork.CompleteAsync(),
            this._emailService.SendEmailAsync(user.Email, "Redefinição de Senha", htmlContent)
        );

        return Unit.Value;
    }
}