namespace CidadeInteligente.Application.ViewModels;

public class UserViewModel {
    public long UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public string MinorName => this.Name.Length > 58 ? this.Name[0..58] : this.Name;
    public string Email { get; private set; } = null!;
    public string Course { get; private set; } = null!;
    public long CourseId { get; private set; }
    public string RoleDescription { get; private set; } = null!;
    public byte Role { get; private set; }
}