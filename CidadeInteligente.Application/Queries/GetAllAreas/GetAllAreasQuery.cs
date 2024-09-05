using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllAreas;

public class GetAllAreasQuery : IRequest<List<Area>> { }