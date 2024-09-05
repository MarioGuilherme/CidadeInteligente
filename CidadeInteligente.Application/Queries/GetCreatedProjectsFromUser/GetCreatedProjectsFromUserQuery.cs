using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCreatedProjectsFromUser;

public class GetCreatedProjectsFromUserQuery(long userId) : IRequest<List<Project>> {
    public long UserId { get; private set; } = userId;
}