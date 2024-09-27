using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQuery(string token) : IRequest<UserDataChangePassword> {
    public string Token { get; private set; } = token;
}