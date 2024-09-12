using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteUserById;

public class DeleteUserByIdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken) {
        User user = await this._unitOfWork.Users.GetByIdAsync(request.UserId)
            ?? throw new UserNotExistException();

        if (await this._unitOfWork.Users.IsInvolvedOrCreatedProjectsAsync(request.UserId))
            throw new UserWithDepedentProjectsException();

        this._unitOfWork.Users.Delete(user);
        await this._unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}