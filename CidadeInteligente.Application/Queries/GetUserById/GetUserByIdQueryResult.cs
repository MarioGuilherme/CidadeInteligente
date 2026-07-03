namespace CidadeInteligente.Application.Queries.GetUserById;

public record GetUserByIdQueryResult(int UserId, int CourseId, string Name, string Email, byte Role);
