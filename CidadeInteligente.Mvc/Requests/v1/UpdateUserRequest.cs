using CidadeInteligente.Domain.Enums;

namespace CidadeInteligente.Mvc.Requests.v1;

public record UpdateUserRequest(int CourseId, string Name, string Email, Role Role);
