using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject {
    public class CreateProjectCommand(string title, long areaId, long courseId, string? description, DateOnly startedAt, DateOnly? finishedAt, List<long> involvedUsers, List<CreateMediaCommand> medias) : IRequest<long> {
        public string Title { get; private set; } = title;
        public long CreatorUserId { get; set; }
        public long AreaId { get; private set; } = areaId;
        public long CourseId { get; private set; } = courseId;
        public string? Description { get; private set; } = description;
        public DateOnly StartedAt { get; private set; } = startedAt;
        public DateOnly? FinishedAt { get; private set; } = finishedAt;
        public List<long> InvolvedUsers { get; private set; } = involvedUsers;
        public List<CreateMediaCommand> Medias { get; private set; } = medias;
    }

    public class CreateMediaCommand(string title, string? description, string extension, byte[] base64) {
        public string Title { get; private set; } = title;
        public string? Description { get; private set; } = description;
        public string Extension { get; private set; } = extension;
        public byte[] Base64 { get; private set; } = base64;
        public long Size => this.Base64.Length;
    }
}