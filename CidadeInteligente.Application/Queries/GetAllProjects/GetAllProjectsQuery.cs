using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<List<ProjectViewModel>> { }