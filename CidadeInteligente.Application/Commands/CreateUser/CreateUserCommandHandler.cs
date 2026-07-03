using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.CreateUser;

public class CreateUserCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Specification<User> specEmailInUse = SpecificationBuilder<User>.Create()
            .Where(u => u.Email == request.Email)
            .Build();

        if (await _unitOfWork.Users.AnyBySpecAsync(specEmailInUse))
        {
            _notification.AddNotification(NotificationType.EmailAlreadyInUse);
            return null;
        }

        User user = new(request.CourseId,
           request.Name,
           request.Email,
           HashPassword(request.Password),
           request.Role);

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await _unitOfWork.Users.CreateAsync(user);
        }, cancellationToken: cancellationToken);

        return user.UserId;
    }
}
