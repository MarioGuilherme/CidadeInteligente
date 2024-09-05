using CidadeInteligente.Application.Commands.CreateUser;
using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.CreateCourse;

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, long> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
        User user = new(
            request.CourseId,
            request.Name,
            request.Email,
            request.Password,
            request.Role
        );

        await this._userRepository.AddAsync(user);

        return user.UserId;
    }
}