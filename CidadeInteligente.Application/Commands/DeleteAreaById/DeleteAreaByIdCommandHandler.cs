using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Specifications;
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
        Specification<Area> specArea = SpecificationBuilder<Area>.Create()
            .Where(a => a.AreaId == request.AreaId)
            .Build();

        if (!await _unitOfWork.Areas.AnyBySpecAsync(specArea))
        {
            _notification.AddNotification(NotificationType.AreaNotFound, [request.AreaId]);
            return null;
        }

        Specification<Project> specDependenteProjectsFromArea = SpecificationBuilder<Project>.Create()
            .Where(p => p.AreaId == request.AreaId)
            .Build();
        if (await _unitOfWork.Projects.AnyBySpecAsync(specDependenteProjectsFromArea))
        {
            _notification.AddNotification(NotificationType.AreaWithDependentProjects, [request.AreaId]);
            return null;
        }

        await _unitOfWork.Areas.DeleteByIdAsync(request.AreaId, cancellationToken);

        return Unit.Value;
    }
}
