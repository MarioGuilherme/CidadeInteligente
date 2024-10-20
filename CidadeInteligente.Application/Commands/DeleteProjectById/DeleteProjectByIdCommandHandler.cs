using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteProjectById;

public class DeleteProjectByIdCommandHandler(IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<DeleteProjectByIdCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit> Handle(DeleteProjectByIdCommand request, CancellationToken cancellationToken) {
        Project project = await this._unitOfWork.Projects.GetByIdAsync(request.ProjectId) ?? throw new ProjectNotExistException();

        if (!(request.UserIdEditor == project.CreatorUserId || project.InvolvedUsers.Any(iu => iu.UserId == request.UserIdEditor)))
            throw new UserIsReadOnlyException();

        foreach (Media media in project.Medias)
            await this._fileStorage.DeleteFileAsync(media.FileName);
        this._unitOfWork.Projects.DeleteProject(project);
        await this._unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}