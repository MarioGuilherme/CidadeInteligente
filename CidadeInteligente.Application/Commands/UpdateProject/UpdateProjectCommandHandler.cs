using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Services;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public class UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IFileStorage fileStorage) : IRequestHandler<UpdateProjectCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken) {
        Project projectDb = await this._unitOfWork.Projects.GetByIdAsync(request.ProjectId, true) ?? throw new ProjectNotExistException();

        if (request.UserIdEditor is not null &&
            !(request.UserIdEditor == projectDb.CreatorUserId || projectDb.InvolvedUsers.Any(iu => iu.UserId == request.UserIdEditor)))
            throw new UserIsReadOnlyException();

        await this._unitOfWork.BeginTransactionAsync();
        projectDb.Update(
            request.AreaId,
            request.CourseId,
            request.Title,
            request.Description,
            request.StartedAt,
            request.FinishedAt
        );
        projectDb.InvolvedUsers.Clear();

        foreach (long userId in request.InvolvedUsers) {
            User? newUserInvolved = await this._unitOfWork.Users.GetByIdAsync(userId, true);
            if (newUserInvolved is null) continue;
            projectDb.InvolvedUsers.Add(newUserInvolved);
        }
        foreach (Media mediaDb in projectDb.Medias) {
            if (request.Medias.Exists(mediaForm => mediaDb.MediaId == mediaForm.MediaId)) continue;
            await this._fileStorage.DeleteFileAsync(mediaDb.FileName);
            this._unitOfWork.Projects.DeleteMedia(mediaDb);
        }

        projectDb.Medias.Clear();
        foreach (UpdateMediaCommand media in request.Medias) {
            if (media.MediaId is null) {
                projectDb.Medias.Add(new(
                    media.Title,
                    media.Description,
                    this._fileStorage.UploadOrUpdateFileAsync($"{Guid.NewGuid():N}.{media.Extension}", media.Base64!).Result,
                    (long)media.Size!
                ));
                continue;
            }

            Media? mediaDb = await this._unitOfWork.Projects.GetMediaById((long)media.MediaId);
            if (mediaDb is null) continue;

            if (media.Base64 is null)
                mediaDb.Update(media.Title, media.Description);
            else {
                await this._fileStorage.UploadOrUpdateFileAsync(mediaDb.FileName, media.Base64);
                mediaDb.Update(media.Title, media.Description, (long)media.Size!);
            }
            projectDb.Medias.Add(mediaDb);
        }

        await this._unitOfWork.CompleteAsync();
        await this._unitOfWork.CommitAsync();
        return Unit.Value;
    }
}