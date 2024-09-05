using CidadeInteligente.Application.Commands.CreateMedia;
using CidadeInteligente.Application.Commands.UpdateProject;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Core.Services;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateProjectCommandHandler(IProjectRepository projectRepository, IUserRepository userRepository, IFileStorage fileStorage) : IRequestHandler<UpdateProjectCommand, Unit?> {
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Unit?> Handle(UpdateProjectCommand request, CancellationToken cancellationToken) {
        Project? projectDb = await this._projectRepository.GetByIdAsync(request.ProjectId, true);
        
        if (projectDb is null) return null;

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
            User? newUserInvolved = await this._userRepository.GetByIdAsync(userId, true);
            if (newUserInvolved is null) continue;
            projectDb.InvolvedUsers.Add(newUserInvolved);
        }
        foreach (Media mediaDb in projectDb.Medias) {
            if (request.Medias.Exists(mediaForm => mediaDb.MediaId == mediaForm.MediaId)) continue;
            await this._fileStorage.DeleteFileAsync(mediaDb.FileName);
            this._projectRepository.DeleteMedia(mediaDb);
        }

        projectDb.Medias.Clear();
        foreach (UpdateMediaCommand media in request.Medias) {
            if (media.MediaId is null) {
                projectDb.Medias.Add(new(
                    media.Title,
                    media.Description,
                    this._fileStorage.UploadFileAsync($"{Guid.NewGuid():N}.{media.Extension}", media.Base64!).Result,
                    (long)media.Size!
                ));
                continue;
            }

            Media? mediaDb = await this._projectRepository.GetMediaById((long)media.MediaId);
            if (mediaDb is null) continue;

            if (media.Base64 is null)
                mediaDb.Update(media.Title, media.Description);
            else {
                string fileName = $"{Guid.NewGuid():N}.{media.Extension}";
                await Task.WhenAll(
                    this._fileStorage.DeleteFileAsync(mediaDb.FileName),
                    this._fileStorage.UploadFileAsync(fileName, media.Base64)
                );
                mediaDb.Update(media.Title, media.Description, fileName, (long)media.Size!);
            }
            projectDb.Medias.Add(mediaDb);
        }

        await this._projectRepository.SaveChangesAsync();
        return Unit.Value;
    }
}