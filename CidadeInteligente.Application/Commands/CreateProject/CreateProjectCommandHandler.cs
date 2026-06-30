using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject;

public class CreateProjectCommandHandler(IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<CreateProjectCommand, long>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<long> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        Project project = new(request.CreatorUserId,
            request.AreaId,
            request.CourseId,
            request.Title,
            request.Description,
            request.StartedAt,
            request.FinishedAt);

        foreach (long involvedUser in request.InvolvedUsers)
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
