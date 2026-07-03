using MediatR;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public record GetRelatedProjectsFromUserQuery(int UserId, int Page) : IRequest<GetRelatedProjectsFromUserQueryResult?> { }
