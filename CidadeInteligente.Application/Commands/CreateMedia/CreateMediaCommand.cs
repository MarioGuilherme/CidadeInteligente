namespace CidadeInteligente.Application.Commands.CreateMedia;

public class CreateMediaCommand {
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Extension { get; set; } = null!;
    public byte[] Base64 { get; set; } = null!;
    public long Size => this.Base64.Length;
}