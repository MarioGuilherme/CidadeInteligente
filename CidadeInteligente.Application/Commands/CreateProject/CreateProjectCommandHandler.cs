using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Notifications;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Core.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace CidadeInteligente.Application.Commands.CreateProject;

public class CreateProjectCommandHandler(INotificationContext notification, IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<CreateProjectCommand, int?>
{
    private readonly INotificationContext _notification = notification;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<int?> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        Specification<Area> specArea = SpecificationBuilder<Area>.Create()
            .Where(a => a.AreaId == request.AreaId)
            .Build();
        Specification<Course> specCourse = SpecificationBuilder<Course>.Create()
            .Where(c => c.CourseId == request.CourseId)
            .Build();

        if (!await _unitOfWork.Areas.AnyBySpecAsync(specArea))
            _notification.AddNotification(NotificationType.AreaNotFound, [request.AreaId]);

        if (!await _unitOfWork.Courses.AnyBySpecAsync(specCourse))
            _notification.AddNotification(NotificationType.CourseNotFound, [request.CourseId]);

        if (_notification.HasNotifications)
            return null;

        Project project = new(request.AreaId,
            request.CourseId,
            request.CreatorUserId,
            request.Title,
            request.Description,
            request.StartedAt,
            request.FinishedAt);

        foreach (int involvedUser in request.InvolvedUsers)
            project.InvolvedUsers.Add(new User(involvedUser));

        foreach (CreateProjectCommand.CreateMediaCommand media in request.Medias)
        {
            await using Stream stream = media.OpenStream();
            string fileName = await _fileStorage.UploadOrUpdateFileAsync($"{Guid.NewGuid():N}{media.Extension}", stream);
            project.Medias.Add(new Media(media.Title, media.Description, fileName));
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.CommitAsync(cancellationToken);

        return project.ProjectId;
    }
}
