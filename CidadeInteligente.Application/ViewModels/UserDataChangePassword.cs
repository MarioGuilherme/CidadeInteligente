namespace CidadeInteligente.Application.ViewModels;

public class UserDataChangePassword(string userName, string token) {
    public string UserName { get; private set; } = userName;
    public string Token { get; private set; } = token;
}