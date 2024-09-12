using MediatR;

namespace CidadeInteligente.Application.Commands.CreateProject {
    public class CreateProjectCommand : IRequest<long> {
        public string Title { get; set; } = null!;
        public long CreatorUserId { get; set; }
        public long AreaId { get; set; }
        public long CourseId { get; set; }
        public string? Description { get; set; }
        public DateOnly StartedAt { get; set; }
        public DateOnly? FinishedAt { get; set; }
        public List<long> InvolvedUsers { get; set; } = [];
        public List<CreateMediaCommand> Medias { get; set; } = [];
    }

    public class CreateMediaCommand {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Extension { get; set; } = null!;
        public byte[] Base64 { get; set; } = null!;
        public long Size => this.Base64.Length;
    }
}