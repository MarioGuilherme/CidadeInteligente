using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Infrastructure.Persistence;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUsers;

public class GetUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<User> users = await _unitOfWork.Users.GetAllAsync();
        return new GetUsersQueryResult(users.Select(u => new GetUsersQueryResult.UserViewModel(
            u.UserId,
            u.Name,
            u.Email,
            u.Course.Description,
            u.CourseId,
            u.Role.GetDescription(),
            (byte)u.Role)));
    }
}
