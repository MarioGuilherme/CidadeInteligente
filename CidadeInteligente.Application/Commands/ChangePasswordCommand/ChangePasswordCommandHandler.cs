using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.ChangePasswordCommand;

public class ChangePasswordCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) {
        User? user = await this._unitOfWork.Users.GetByTokenRecoverPasswordAsync(request.Token) ?? throw new UserNotExistException();

        if (DateTime.Now > user.TokenRecoverPasswordExpiration) {
            user.RemovePasswordResetTokenInformation();
            await this._unitOfWork.CompleteAsync();
            throw new TokenRecoverPasswordExpiredException();
        }

        user.UpdatePassword(request.NewPassword);

        await this._unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}