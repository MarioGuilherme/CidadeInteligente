using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<int?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByEmailSpecification = UserSpecifications.GetByEmailAndExceptUserId(request.Email).Build();
        if (await _unitOfWork.Users.AnyBySpecAsync(getUserByEmailSpecification, cancellationToken))
        {
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        User user = new(request.CourseId,
           request.Name,
           request.Email,
           _passwordHasher.Hash(request.Password),
           request.Role);

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await _unitOfWork.Users.CreateAsync(user, ct);
        }, cancellationToken: cancellationToken);

        return user.UserId;
    }
}
