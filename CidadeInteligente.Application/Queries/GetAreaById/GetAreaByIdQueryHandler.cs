using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Areas;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAreaById;

public class GetAreaByIdQueryHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<GetAreaByIdQuery, GetAreaByIdQueryResult?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetAreaByIdQueryResult?> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
    {
        Specification<Area, GetAreaByIdQueryResult?> getAreaByIdSpec = AreaSpecifications.GetById(request.AreaId)
            .WithProjection<GetAreaByIdQueryResult>(a => new(a.AreaId, a.Description));

        GetAreaByIdQueryResult? area = await _unitOfWork.Areas.GetBySpecAsync(getAreaByIdSpec, cancellationToken);
        if (area is null)
        {
            _notification.AddNotification(NotificationType.AreaNotFound, [request.AreaId]);
            return null;
        }

        return area;
    }
}
