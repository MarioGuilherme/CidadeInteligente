using AutoMapper;
using CidadeInteligente.Application.Validators;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using FluentValidation;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserByTokenRecoverPassword;

public class GetUserByTokenRecoverPasswordQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserByTokenRecoverPasswordQuery, UserDataChangePassword> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDataChangePassword> Handle(GetUserByTokenRecoverPasswordQuery request, CancellationToken cancellationToken) {
        await new GetUserByTokenRecoverPasswordQueryValidator().ValidateAndThrowAsync(request, cancellationToken);

        User? user = await this._unitOfWork.Users.GetByTokenRecoverPasswordAsync(request.Token) ?? throw new UserNotExistException();

        if (DateTime.Now > user.TokenRecoverPasswordExpiration) {
            user.RemovePasswordResetTokenInformation();
            await this._unitOfWork.CompleteAsync();
            throw new TokenRecoverPasswordExpiredException();
        }

        return this._mapper.Map<UserDataChangePassword>(user);
    }
}