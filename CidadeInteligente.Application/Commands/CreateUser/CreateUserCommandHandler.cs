using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, long> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
        if (await this._unitOfWork.Users.IsEmailInUseExceptByUserId(request.Email))
            throw new EmailAlreadyInUseException();

        User user = new(
            request.CourseId,
            request.Name,
            request.Email,
            request.Password,
            request.Role
        );

        await this._unitOfWork.Users.AddAsync(user);
        await this._unitOfWork.CompleteAsync();

        return user.UserId;
    }
}