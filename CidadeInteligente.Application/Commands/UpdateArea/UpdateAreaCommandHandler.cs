using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
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
        Area? area = await _unitOfWork.Areas.GetByIdAsync(request.AreaId);

        if (area is null)
        {
            Log.Warning("Area with ID {AreaId} ​​not found.", request.AreaId);
            _notification.AddNotification(NotificationType.AreaNotFound);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        area.Update(request.Description);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
