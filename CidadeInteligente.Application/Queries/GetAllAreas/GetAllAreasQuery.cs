using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllAreas;

public class GetAllAreasQuery : IRequest<List<AreaViewModel>> { }