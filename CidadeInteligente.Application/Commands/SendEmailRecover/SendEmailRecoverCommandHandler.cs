using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.SendEmailRecover;

public class SendEmailRecoverCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SendEmailRecoverCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(SendEmailRecoverCommand request, CancellationToken cancellationToken) {
        User? user = await this._unitOfWork.Users.GetByEmailAsync(request.Email, true);

        if (user is null) return Unit.Value;

        user.SaveNewTokenToRecoverPassword();

        await this._unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}