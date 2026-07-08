using CidadeInteligente.Domain.Enums;

namespace CidadeInteligente.Mvc.Requests.v1;

public record CreateUserRequest(int CourseId, string Name, string Email, string Password, string ConfirmPassword, Role Role);
