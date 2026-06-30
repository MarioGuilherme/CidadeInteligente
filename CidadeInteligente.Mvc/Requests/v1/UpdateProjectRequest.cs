namespace CidadeInteligente.Mvc.Requests.v1;

public record UpdateProjectRequest(string Title,
    long AreaId,
    long CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<long> InvolvedUsers,
    IEnumerable<UpdateProjectRequest.UpdateMediaRequest> Medias)
{
    public record UpdateMediaRequest(long? MediaId, string Title, string? Description, IFormFile File);
}
