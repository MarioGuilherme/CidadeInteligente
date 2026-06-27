using MediatR;

namespace CidadeInteligente.UI.Requests;

public record CreateProjectRequest(string Title,
    long AreaId,
    long CourseId,
    long CreatorUserId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<long> InvolvedUsers,
    IEnumerable<CreateProjectRequest.CreateMediaRequest> Medias) : IRequest<long>
{
    public record CreateMediaRequest(string Title, string? Description, string Extension, byte[] Base64)
    {
        public long Size => Base64.Length;
    }
}
