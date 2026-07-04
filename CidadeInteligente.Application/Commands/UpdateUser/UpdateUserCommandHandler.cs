using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        Specification<User> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.UserId == request.UserId)
            .AsEditable()
            .Build();

        User? user = await _unitOfWork.Users.GetBySpecAsync(spec);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        Specification<User> specEmailInUse = SpecificationBuilder<User>.Create()
            .Where(u => u.UserId != request.UserId && u.Email == request.Email)
            .Build();

        if (await _unitOfWork.Users.AnyBySpecAsync(specEmailInUse))
        {
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => user.Update(request.CourseId, request.Name, request.Email, request.Role),
            cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
