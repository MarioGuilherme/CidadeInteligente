using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Repositories;
using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateCourse;

public class UpdateCourseCommandHandler(ICourseRepository courseRepository) : IRequestHandler<UpdateCourseCommand, Unit?> {
    private readonly ICourseRepository _courseRepository = courseRepository;

    public async Task<Unit?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken) {
        Course? course = await this._courseRepository.GetByIdAsync(request.CourseId, true);

        if (course is null) return null;

        course.Update(request.Description);

        await this._courseRepository.SaveChangesAsync();
        return Unit.Value;
    }
}