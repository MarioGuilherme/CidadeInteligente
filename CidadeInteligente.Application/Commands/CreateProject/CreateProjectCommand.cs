using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject;

public record CreateProjectCommand(string Title,
    long AreaId,
    long CourseId,
    long CreatorUserId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<long> InvolvedUsers,
    IEnumerable<CreateProjectCommand.CreateMediaCommand> Medias) : IRequest<long>
{
    public record CreateMediaCommand(string Title, string? Description, string Extension, Func<Stream> OpenStream);
}
