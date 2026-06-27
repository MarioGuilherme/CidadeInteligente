using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<LoginUserCommand, LoginUserCommandResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<LoginUserCommandResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User possibleUser = await _unitOfWork.Users.GetByEmailAsync(request.Email) ?? throw new EmailOrPasswordNotMatchException();

        if (!Verify(request.Password, possibleUser.Password))
            throw new EmailOrPasswordNotMatchException();

        return new(possibleUser.UserId, possibleUser.Role.GetDescription());
    }
}
