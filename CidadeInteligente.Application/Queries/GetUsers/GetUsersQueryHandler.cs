using CidadeInteligente.Application.Extensions;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using MediatR;

namespace CidadeInteligente.Application.Queries.GetUsers;

public class GetUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersQuery, GetUsersQueryResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<GetUsersQueryResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        Specification<User, GetUsersQueryResult.UserViewModel> getUsersSpec = SpecificationBuilder<User>.Create()
            .WithProjection<GetUsersQueryResult.UserViewModel>(u => new(u.UserId,
                u.Name,
                u.Email,
                u.Course.Description,
                u.Role.GetDescription()))!;

        IEnumerable<GetUsersQueryResult.UserViewModel> users = await _unitOfWork.Users.GetAllBySpecAsync(getUsersSpec, cancellationToken);
        return new GetUsersQueryResult(users);
    }
}
