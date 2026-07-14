using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService) : IRequestHandler<SendEmailRecoverCommand, Unit?>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailService _emailService = emailService;

    public async Task<Unit?> Handle(SendEmailRecoverCommand request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByEmailSpecification = UserSpecifications.GetByEmailAndExceptUserId(request.Email).Build();
        User? user = await _unitOfWork.Users.GetBySpecAsync(getUserByEmailSpecification, cancellationToken);
        if (user is null) return Unit.Value;

        await _unitOfWork.ExecuteInTransactionAsync(user.SaveNewTokenToRecoverPassword, cancellationToken: cancellationToken);
        await _emailService.SendRecoverPasswordEmailAsync(user.Email, user.Name, request.BaseUrl, user.TokenRecoverPassword!);

        return Unit.Value;
    }
}
