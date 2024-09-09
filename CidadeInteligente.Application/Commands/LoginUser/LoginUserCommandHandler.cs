using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<LoginUserCommand, UserViewModel?> {
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserViewModel?> Handle(LoginUserCommand request, CancellationToken cancellationToken) {
        User? possibleUser = await this._userRepository.GetByEmailAsync(request.Email);

        if (possibleUser is null) return null;

        if (!Verify(request.Password, possibleUser.Password)) return null;

        return this._mapper.Map<UserViewModel>(possibleUser);
    }
}