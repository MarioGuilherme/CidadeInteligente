using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Notifications;
using CidadeInteligente.Domain.Services;
using CidadeInteligente.Domain.Specifications;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using System.Collections.Concurrent;

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

        ConcurrentBag<Media> uploadedMedias = [];

        await _unitOfWork.ExecuteInTransactionAsync(async _ =>
        {
            foreach (int involvedUserId in request.InvolvedUsers)
            {
                Specification<User> specNewInvolvedUser = SpecificationBuilder<User>.Create()
                    .Where(u => u.UserId == involvedUserId)
                    .AsEditable()
                    .Build();

                User? newUserInvolved = await _unitOfWork.Users.GetBySpecAsync(specNewInvolvedUser);
                if (newUserInvolved is null)
                {
                    _notification.AddNotification(NotificationType.UserNotFound, [involvedUserId]);
                    continue;
                }
                project.InvolvedUsers.Add(newUserInvolved);
            }

            await Task.WhenAll(request.Medias.Select(async m =>
            {
                await using Stream stream = m.OpenStream();
                string fileName = Guid.NewGuid().ToString("N");
                Media media = new(m.Title, m.Description, fileName, m.MimeType);
                await _fileStorage.UploadOrUpdateFileAsync(fileName, stream, cancellationToken);
                uploadedMedias.Add(media);
            }));

            foreach (Media uploadedMedia in uploadedMedias)
                project.Medias.Add(uploadedMedia);

            await _unitOfWork.Projects.AddAsync(project);
        },
        onRollback: async ct =>
        {
            await Task.WhenAll(uploadedMedias.Select(m => _fileStorage.DeleteFileAsync(m.FileName, ct)));
        }, cancellationToken);

        return project.ProjectId;
    }
}
