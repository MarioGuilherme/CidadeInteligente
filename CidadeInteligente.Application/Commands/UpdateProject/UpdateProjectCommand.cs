using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateProject {
    public class UpdateProjectCommand(string title, long areaId, long courseId, string? description, DateOnly startedAt, DateOnly? finishedAt, List<long> involvedUsers, List<UpdateMediaCommand> medias) : IRequest<Unit> {
        public long ProjectId { get; set; }
        public long? UserIdEditor { get; set; }
        public string Title { get; private set; } = title;
        public long AreaId { get; private set; } = areaId;
        public long CourseId { get; private set; } = courseId;
        public string? Description { get; private set; } = description;
        public DateOnly StartedAt { get; private set; } = startedAt;
        public DateOnly? FinishedAt { get; private set; } = finishedAt;
        public List<long> InvolvedUsers { get; private set; } = involvedUsers;
        public List<UpdateMediaCommand> Medias { get; private set; } = medias;
    }

    public class UpdateMediaCommand(long? mediaId, string title, string? description, string extension, string? path, byte[]? base64) {
        public long? MediaId { get; private set; } = mediaId;
        public string Title { get; private set; } = title;
        public string? Description { get; private set; } = description;
        public string Extension { get; private set; } = extension;
        public string? Path { get; private set; } = path;
        public byte[]? Base64 { get; private set; } = base64;
        public long? Size => this.Base64?.Length;
    }
}