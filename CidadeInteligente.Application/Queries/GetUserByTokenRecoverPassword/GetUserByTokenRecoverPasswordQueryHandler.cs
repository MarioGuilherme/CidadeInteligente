using CidadeInteligente.Application.Validators;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using FluentValidation;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByTokenRecoverPasswordQuery, GetUserByTokenRecoverPasswordQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByTokenRecoverPasswordQueryResult> Handle(GetUserByTokenRecoverPasswordQuery request, CancellationToken cancellationToken)
    {
        await new GetUserByTokenRecoverPasswordQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        User? user = await _unitOfWork.Users.GetByTokenRecoverPasswordAsync(request.Token) ?? throw new UserNotExistException();

        if (DateTime.Now > user.TokenRecoverPasswordExpiration)
        {
            user.RemovePasswordResetTokenInformation();
            await _unitOfWork.CompleteAsync();
            throw new TokenRecoverPasswordExpiredException();
        }

        return new(user.Name, user.TokenRecoverPassword!);
    }
}
