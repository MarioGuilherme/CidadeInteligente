namespace CidadeInteligente.Application.ViewModels;

public class LoginUserViewModel(string email, string token) {
    public string Email { get; private set; } = email;
    public string Token { get; private set; } = token;
}