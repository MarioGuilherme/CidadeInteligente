using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQuery(long userId) : IRequest<UserViewModel?> {
    public long UserId { get; private set; } = userId;
}