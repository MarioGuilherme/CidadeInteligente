using CidadeInteligente.Application.ViewModels;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<UserViewModel>> { }