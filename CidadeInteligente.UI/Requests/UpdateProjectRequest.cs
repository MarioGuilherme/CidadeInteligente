namespace CidadeInteligente.UI.Requests;

public record UpdateProjectRequest(string Title,
    long AreaId,
    long CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<long> InvolvedUsers,
    IEnumerable<UpdateProjectRequest.UpdateMediaRequest> Medias)
{
    public record UpdateMediaRequest(long? MediaId, string Title, string? Description, string Extension, string? Path, byte[]? Base64)
    {
        public long? Size => Base64?.Length;
    }
}
