using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserViewModel?> {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserViewModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
        User? user = await this._userRepository.GetByIdAsync(request.UserId);
        return this._mapper.Map<UserViewModel?>(user);
    }
}