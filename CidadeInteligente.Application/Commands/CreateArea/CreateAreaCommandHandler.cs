using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateArea;

public class CreateAreaCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<CreateAreaCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<int?> Handle(CreateAreaCommand request, CancellationToken cancellationToken)
    {
        Specification<Area> specCourseDescriptionInUse = SpecificationBuilder<Area>.Create()
            .Where(a => a.Description == request.Description)
            .Build();

        if (await _unitOfWork.Areas.AnyBySpecAsync(specCourseDescriptionInUse))
        {
            _notification.AddNotification(NotificationType.AreaAlreadyExists, [request.Description]);
            return null;
        }

        Area area = new(request.Description);

        await _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            await _unitOfWork.Areas.AddAsync(area);
        }, cancellationToken: cancellationToken);

        return area.AreaId;
    }
}
