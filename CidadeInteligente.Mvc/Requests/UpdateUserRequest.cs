using CidadeInteligente.Core.Enums;

namespace CidadeInteligente.Mvc.Requests;

public record UpdateUserRequest(long CourseId, string Name, string Email, Role Role);
