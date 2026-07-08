using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjects;

public record GetProjectsQuery(int Page) : IRequest<GetProjectsQueryResult> { }
