using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject;

public record CreateProjectCommand(string Title,
    int AreaId,
    int CourseId,
    int CreatorUserId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<int> InvolvedUsers,
    IEnumerable<CreateProjectCommand.CreateMediaCommand> Medias) : IRequest<int?>
{
    public record CreateMediaCommand(string Title, string? Description, string Extension, Func<Stream> OpenStream);
}
