using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        //Specification<User> spec = SpecificationBuilder<User>.Create()
        //    .Where(u => u.UserId == request.UserId)
        //    .AsEditable()
        //    .Build();

        //User? user = await _unitOfWork.Users.GetProjectionBySpecAsync(spec);

        Specification<User> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.UserId == request.UserId)
            .AsEditable()
            .Build();

        User? user = await _unitOfWork.Users.GetBySpecAsync(spec);
        if (user is null)
        {
            _notification.AddNotification(NotificationType.UserNotFound);
            return null;
        }

        Specification<User> specEmailInUse = SpecificationBuilder<User>.Create()
            .Where(u => u.UserId != request.UserId && u.Email == request.Email)
            .Build();

        bool isEmailAlreadyInUse = await _unitOfWork.Users.AnyBySpecAsync(specEmailInUse);
        if (isEmailAlreadyInUse)
        {
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        user.Update(request.CourseId, request.Name, request.Email, request.Role);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
