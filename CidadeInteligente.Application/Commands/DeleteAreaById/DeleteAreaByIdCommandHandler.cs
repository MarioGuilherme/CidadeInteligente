using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteAreaByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteAreaByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteAreaByIdCommand request, CancellationToken cancellationToken)
    {
        Area? area = await _unitOfWork.Areas.GetByIdAsync(request.AreaId);
        if (area is null)
        {
            Log.Warning("Area with ID {AreaId} ​​not found.", request.AreaId);
            _notification.AddNotification(NotificationType.AreaNotFound);
            return null;
        }

        if (await _unitOfWork.Areas.HaveProjectsAsync(request.AreaId))
        {
            Log.Warning("Area with ID {AreaId} has dependent projects and cannot be deleted.", request.AreaId);
            _notification.AddNotification(NotificationType.AreaWithDependentProjects);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        _unitOfWork.Areas.Delete(area);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
