using MediatR;

namespace CidadeInteligente.Application.Queries.GetRelatedProjectsFromUser;

public record GetRelatedProjectsFromUserQuery(long UserId, int Page) : IRequest<GetRelatedProjectsFromUserQueryResult> { }
