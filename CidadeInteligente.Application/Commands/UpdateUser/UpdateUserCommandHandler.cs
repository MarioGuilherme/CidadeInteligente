﻿using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Unit> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
        User user = await this._unitOfWork.Users.GetByIdAsync(request.UserId, true) ?? throw new UserNotExistException();

        if (await this._unitOfWork.Users.IsEmailInUseExceptByUserId(request.Email, user.UserId))
            throw new EmailAlreadyInUseException();

        user.Update(request.CourseId, request.Name, request.Email, request.Role);

        await this._unitOfWork.CompleteAsync();
        return Unit.Value;
    }
}