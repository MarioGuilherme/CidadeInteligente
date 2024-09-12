using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Core.Repositories;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Application.Commands.LoginUser;

public class LoginUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<LoginUserCommand, LoginViewModel> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<LoginViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken) {
        User possibleUser = await this._unitOfWork.Users.GetByEmailAsync(request.Email) ?? throw new EmailOrPasswordNotMatchException();

        if (!Verify(request.Password, possibleUser.Password))
            throw new EmailOrPasswordNotMatchException();

        return this._mapper.Map<LoginViewModel>(possibleUser);
    }
}