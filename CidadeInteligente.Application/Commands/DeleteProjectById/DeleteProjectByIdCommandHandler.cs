using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Services;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public class DeleteProjectByIdCommandHandler(IProjectRepository projectRepository, IFileStorage fileStorage) : IRequestHandler<DeleteProjectByIdCommand, Unit?> {
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit?> Handle(DeleteProjectByIdCommand request, CancellationToken cancellationToken) {
        Project? project = await this._projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null) return null;

        await this._projectRepository.DeleteProjectAsync(project);
        foreach (Media media in project.Medias)
            await this._fileStorage.DeleteFileAsync(media.FileName);

        return Unit.Value;
    }
}