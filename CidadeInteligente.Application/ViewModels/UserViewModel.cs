namespace CidadeInteligente.Application.ViewModels;

public class UserViewModel(long userId, string name, string email, string course, long courseId, string roleDescription, byte role)
{
    public long UserId { get; private set; } = userId;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Course { get; private set; } = course;
    public long CourseId { get; private set; } = courseId;
    public string RoleDescription { get; private set; } = roleDescription;
    public byte Role { get; private set; } = role;
    public string MinorName => this.Name.Length > 58 ? this.Name[0..58] : this.Name;
}