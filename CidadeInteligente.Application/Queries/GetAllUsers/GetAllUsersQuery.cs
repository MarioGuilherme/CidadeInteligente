using CidadeInteligente.Core.Entities;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<User>> { }