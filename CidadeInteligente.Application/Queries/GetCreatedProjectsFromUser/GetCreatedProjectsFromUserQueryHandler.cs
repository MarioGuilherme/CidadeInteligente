using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetCreatedProjectsFromUser;

public class GetCreatedProjectsFromUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetCreatedProjectsFromUserQuery, List<Project>> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<List<Project>> Handle(GetCreatedProjectsFromUserQuery request, CancellationToken cancellationToken) {
        List<Project> projects = await this._userRepository.GetCreatedProjectsFromUser(request.UserId);
        return projects;
    }
}