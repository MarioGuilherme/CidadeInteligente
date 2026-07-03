using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByIdQueryResult?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<User, GetUserByIdQueryResult?> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.UserId == request.UserId)
            .WithProjection(u => new GetUserByIdQueryResult(u.UserId,
                u.CourseId,
                u.Name,
                u.Email,
                (byte)u.Role));

        GetUserByIdQueryResult? getUserByIdQueryResult = await _unitOfWork.Users.GetBySpecAsync(spec);
        if (getUserByIdQueryResult is null)
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        return getUserByIdQueryResult;
    }
}
