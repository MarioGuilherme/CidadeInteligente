using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        Specification<User> getUserByIdSpec = UserSpecifications.GetById(request.UserId).Build();
        User? user = await _unitOfWork.Users.GetBySpecAsync(getUserByIdSpec, cancellationToken);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        Specification<User> getUserByEmailExceptUserIdSpec = UserSpecifications.GetByEmailAndExceptUserId(request.Email, request.UserId).Build();
        if (await _unitOfWork.Users.AnyBySpecAsync(getUserByEmailExceptUserIdSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => user.Update(request.CourseId, request.Name, request.Email, request.Role),
            cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
