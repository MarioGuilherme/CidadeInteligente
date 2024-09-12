using MediatR;

namespace CidadeInteligente.Application.Commands.UpdateProject {
    public class UpdateProjectCommand : IRequest<Unit> {
        public long? UserIdEditor { get; set; }
        public long ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public long AreaId { get; set; }
        public long CourseId { get; set; }
        public string? Description { get; set; }
        public DateOnly StartedAt { get; set; }
        public DateOnly? FinishedAt { get; set; }
        public List<long> InvolvedUsers { get; set; } = [];
        public List<UpdateMediaCommand> Medias { get; set; } = [];
    }

    public class UpdateMediaCommand {
        public long? MediaId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Extension { get; set; } = null!;
        public string? Path { get; set; }
        public byte[]? Base64 { get; set; }
        public long? Size => this.Base64?.Length;
    }
}