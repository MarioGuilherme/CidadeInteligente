using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserCommand, Unit?> {
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit?> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
        User? user = await this._userRepository.GetByIdAsync(request.UserId, true);

        if (user is null) return null;

        user.Update(request.CourseId, request.Name, request.Email, request.Role);

        await this._userRepository.SaveChangesAsync();
        return Unit.Value;
    }
}