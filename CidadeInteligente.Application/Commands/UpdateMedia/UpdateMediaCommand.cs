namespace CidadeInteligente.Application.Commands.CreateMedia;

public class UpdateMediaCommand {
    public long? MediaId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Extension { get; set; } = null!;
    public string? Path { get; set; }
    public byte[]? Base64 { get; set; }
    public long? Size => this.Base64?.Length;
}