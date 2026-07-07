using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Areas;
using CidadeInteligente.Domain.Specifications.Courses;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateAreaCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateAreaCommand request, CancellationToken cancellationToken)
    {
        Specification<Area> getAreaByIdSpec = AreaSpecifications.GetById(request.AreaId).Build();
        Area? area = await _unitOfWork.Areas.GetBySpecAsync(getAreaByIdSpec, cancellationToken);
        if (area is null)
        {
            _notification.AddNotification(NotificationType.AreaNotFound);
            return null;
        }

        Specification<Area> specCourseDescriptionInUse = SpecificationBuilder<Area>.Create()
            .Where(a => a.AreaId != request.AreaId && a.Description == request.Description)
            .Build();
        if (await _unitOfWork.Areas.AnyBySpecAsync(specCourseDescriptionInUse, cancellationToken))
        {
            _notification.AddNotification(NotificationType.AreaAlreadyExists, [request.Description]);
            return null;
        }

        await _unitOfWork.ExecuteInTransactionAsync(() => area.Update(request.Description), cancellationToken: cancellationToken);

        return Unit.Value;
    }
}
