using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateProject;

public record UpdateProjectCommand(long ProjectId,
    long? CurrentUserId,
    string Title,
    long AreaId,
    long CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<long> InvolvedUsers,
    IEnumerable<UpdateProjectCommand.UpdateMediaCommand> Medias) : IRequest<Unit?>
{
    public record UpdateMediaCommand(long? MediaId, string Title, string? Description, string Extension, string? Path, byte[]? Base64)
    {
        public long? Size => Base64?.Length;
    }
}
