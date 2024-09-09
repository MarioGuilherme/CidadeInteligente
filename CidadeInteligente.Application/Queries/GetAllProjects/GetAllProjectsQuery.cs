using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Models;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<PaginationResult<ProjectViewModel>> {
    public int Page { get; set; } = 1;
}