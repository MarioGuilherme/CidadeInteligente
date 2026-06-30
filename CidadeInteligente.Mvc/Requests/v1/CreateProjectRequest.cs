namespace CidadeInteligente.Mvc.Requests.v1;

public record CreateProjectRequest(string Title,
    int AreaId,
    int CourseId,
    string? Description,
    DateOnly StartedAt,
    DateOnly? FinishedAt,
    IEnumerable<int> InvolvedUsers,
    IEnumerable<CreateProjectRequest.CreateMediaRequest> Medias)
{
    public record CreateMediaRequest(string Title, string? Description, IFormFile File);
}
