using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Users;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUserByIdQueryResult?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<User, GetUserByIdQueryResult?> getUserByIdSpec = UserSpecifications.GetById(request.UserId)
            .WithProjection<GetUserByIdQueryResult>(u => new(u.UserId,
                u.CourseId,
                u.Name,
                u.Email,
                (byte)u.Role));

        GetUserByIdQueryResult? getUserByIdQueryResult = await _unitOfWork.Users.GetBySpecAsync(getUserByIdSpec, cancellationToken);
        if (getUserByIdQueryResult is null)
        {
            _notification.AddNotification(NotificationType.UserNotFound, [request.UserId]);
            return null;
        }

        return getUserByIdQueryResult;
    }
}
