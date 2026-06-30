using MediatR;

namespace CidadeInteligente.Mvc.Requests.v1;

public record CreateProjectRequest(string Title,
    long AreaId,
    long CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<long> InvolvedUsers,
    IEnumerable<CreateProjectRequest.CreateMediaRequest> Medias) : IRequest<long>
{
    public record CreateMediaRequest(string Title, string? Description, IFormFile File);
}
