namespace CidadeInteligente.Application.ViewModels;

public class LoginViewModel(long userId, string role) {
    public long UserId { get; private set; } = userId;
    public string Role { get; private set; } = role;
}