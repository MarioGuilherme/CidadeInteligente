using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject;

public record CreateProjectCommand(int CurrentUserId,
    string Title,
    int AreaId,
    int CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<int> InvolvedUsers,
    IEnumerable<CreateProjectCommand.CreateMediaCommand> Medias) : IRequest<int?>
{
    public record CreateMediaCommand(string Title, string? Description, string MimeType, long FileSize, Func<Stream> OpenStream);
}
