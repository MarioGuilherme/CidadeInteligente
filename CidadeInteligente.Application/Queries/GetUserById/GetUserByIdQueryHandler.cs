using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByIdQueryResult?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await _unitOfWork.Users.GetByIdAsync(request.UserId);

        if (user is null)
        {
            Log.Warning("User with ID {UserId} not found.", request.UserId);
            _notification.AddNotification(NotificationType.UserNotFound);
            return null;
        }

        return new(user.UserId,
            user.Name,
            user.Email,
            user.Course.CourseId,
            (byte)user.Role);
    }
}