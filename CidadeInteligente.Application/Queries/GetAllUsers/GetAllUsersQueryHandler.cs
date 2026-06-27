using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Application.ViewModels;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersQuery, List<UserViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        List<User> users = await this._unitOfWork.Users.GetAllAsync();
        return [.. users.Select(u => new UserViewModel(
            u.UserId,
            u.Name,
            u.Email,
            u.Course.Description,
            u.CourseId,
            u.Role.GetDescription(),
            (byte)u.Role))];
    }
}