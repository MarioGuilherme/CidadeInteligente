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
            .AsEditable()
            .Build();

        Area? area = await _unitOfWork.Areas.GetBySpecAsync(specArea);
        if (area is null)
        {
            _notification.AddNotification(NotificationType.AreaNotFound, [request.AreaId]);
            return null;
        }

        Specification<Project> specProjectsFromArea = SpecificationBuilder<Project>.Create()
            .Where(p => p.AreaId == request.AreaId)
            .Build();
        bool areaHasDependentProjects = await _unitOfWork.Projects.AnyBySpecAsync(specProjectsFromArea);
        if (areaHasDependentProjects)
        {
            _notification.AddNotification(NotificationType.AreaWithDependentProjects, [request.AreaId]);
            return null;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _unitOfWork.Areas.DeleteAsync(area);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
