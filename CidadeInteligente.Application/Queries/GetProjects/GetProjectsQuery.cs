using MediatR;

namespace CidadeInteligente.Application.Queries.GetProjects;

public record GetProjectsQuery(int Page = 1) : IRequest<GetProjectsQueryResult> { }
