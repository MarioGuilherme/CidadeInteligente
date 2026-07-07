using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Domain.Specifications.Areas;
using CidadeInteligente.Domain.Specifications.Projects;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteAreaById;

public class DeleteAreaByIdCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork) : IRequestHandler<DeleteAreaByIdCommand, Unit?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit?> Handle(DeleteAreaByIdCommand request, CancellationToken cancellationToken)
    {

        Specification<Area> getAreaByIdSpec = AreaSpecifications.GetById(request.AreaId).Build();
        if (!await _unitOfWork.Areas.AnyBySpecAsync(getAreaByIdSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.AreaNotFound, [request.AreaId]);
            return null;
        }

        Specification<Project> getProjectsByAreaIdSpec = ProjectSpecifications.GetByAreaId(request.AreaId).Build();
        if (await _unitOfWork.Projects.AnyBySpecAsync(getProjectsByAreaIdSpec, cancellationToken))
        {
            _notification.AddNotification(NotificationType.AreaWithDependentProjects, [request.AreaId]);
            return null;
        }

        await _unitOfWork.Areas.DeleteByPredicateAsync(a => a.AreaId == request.AreaId, cancellationToken);

        return Unit.Value;
    }
}
