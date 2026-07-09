namespace CidadeInteligente.Mvc.Requests.v1;

public record UpdateProjectRequest(string Title,
    int AreaId,
    int CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<int> InvolvedUsers,
    IEnumerable<UpdateProjectRequest.UpdateMediaRequest> Medias)
{
    public record UpdateMediaRequest(int? MediaId, string Title, string? Description, IFormFile? File);
}
