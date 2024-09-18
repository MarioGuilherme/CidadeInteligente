using AutoMapper;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Exceptions;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserViewModel> {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) {
        User user = await this._unitOfWork.Users.GetByIdAsync(request.UserId) ?? throw new UserNotExistException();
        return this._mapper.Map<UserViewModel>(user);
    }
}