using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.UpdateArea;

public class UpdateAreaCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<UpdateAreaCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(UpdateAreaCommand request, CancellationToken cancellationToken)
    {
        //Area? area = await _unitOfWork.Areas.GetByIdAsync(request.AreaId, true);
        Specification<Area> specArea = SpecificationBuilder<Area>.Create()
            .Where(a => a.AreaId == request.AreaId)
            .AsEditable()
            .Build();

        Area? area = await _unitOfWork.Areas.GetBySpecAsync(specArea);
        if (area is null)
        {
            _notification.AddNotification(NotificationType.AreaNotFound);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        area.Update(request.Description);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
