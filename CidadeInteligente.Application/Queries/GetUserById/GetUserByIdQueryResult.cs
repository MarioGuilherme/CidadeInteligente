namespace CidadeInteligente.Application.Queries.GetUserById;

public record GetUserByIdQueryResult(long UserId, string Name, string Email, string Course, long CourseId, string RoleDescription, byte Role);
