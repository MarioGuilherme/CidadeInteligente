namespace CidadeInteligente.Application.Queries.GetUserById;

public record GetUserByIdQueryResult(int UserId, string Name, string Email, int CourseId, byte Role);
