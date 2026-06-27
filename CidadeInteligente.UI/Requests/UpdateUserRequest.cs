using CidadeInteligente.Core.Enums;

namespace CidadeInteligente.UI.Requests;

public record UpdateUserRequest(long CourseId, string Name, string Email, Role Role);
