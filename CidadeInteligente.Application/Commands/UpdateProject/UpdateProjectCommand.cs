using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public record UpdateProjectCommand(int ProjectId,
    int CurrentUserId,
    string Title,
    int AreaId,
    int CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<int> InvolvedUsers,
    IEnumerable<UpdateProjectCommand.UpdateMediaCommand> Medias) : IRequest<Unit?>
{
    public record UpdateMediaCommand(int? MediaId, string Title, string? Description, string MimeType, long FileSize, Func<Stream> OpenStream);
}
