using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreaByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetAreaByIdQuery, GetAreaByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetAreaByIdQueryResult?> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
    {
        Area? area = await _unitOfWork.Areas.GetByIdAsync(request.AreaId);
        if (area is null)
        {
            Log.Warning("Area with ID {AreaId} ​​not found.", request.AreaId);
            _notification.AddNotification(NotificationType.AreaNotFound);
            return null;
        }

        return new(area.AreaId, area.Description);
    }
}
