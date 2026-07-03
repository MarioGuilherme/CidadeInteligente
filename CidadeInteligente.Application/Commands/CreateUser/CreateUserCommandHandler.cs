using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Specification<User> spec = SpecificationBuilder<User>.Create()
            .Where(u => u.Email == request.Email)
            .Build();

        bool isEmailAlreadyInUse = await _unitOfWork.Users.AnyBySpecAsync(spec);
        if (isEmailAlreadyInUse)
        {
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        User user = new(request.CourseId,
            request.Name,
            request.Email,
            BCrypt.Net.BCrypt.HashPassword(request.Password),
            request.Role);
        await _unitOfWork.Users.CreateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return user.UserId;
    }
}
