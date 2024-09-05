using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQuery(long userId) : IRequest<User?> {
    public long UserId { get; private set; } = userId;
}