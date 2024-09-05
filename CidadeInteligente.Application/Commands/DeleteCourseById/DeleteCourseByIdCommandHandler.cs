using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.DeleteCourseById;

public class DeleteCourseByIdCommandHandler(ICourseRepository courseRpository) : IRequestHandler<DeleteCourseByIdCommand, Unit?> {
    private readonly ICourseRepository _courseRpository = courseRpository;

    public async Task<Unit?> Handle(DeleteCourseByIdCommand request, CancellationToken cancellationToken) {
        Course? course = await this._courseRpository.GetByIdAsync(request.CourseId);

        if (course is null) return null;

        await this._courseRpository.DeleteByIdAsync(course);

        return Unit.Value;
    }
}