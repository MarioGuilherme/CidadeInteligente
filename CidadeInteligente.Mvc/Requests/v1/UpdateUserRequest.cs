using CidadeInteligente.Core.Enums;

namespace CidadeInteligente.Mvc.Requests.v1;

public record UpdateUserRequest(long CourseId, string Name, string Email, Role Role);
