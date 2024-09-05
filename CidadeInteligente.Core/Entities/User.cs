using CidadeInteligente.Core.Enums;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Core.Entities;

public class User {
    public long UserId { get; private set; }
    public long CourseId { get; set; }
    public Course Course { get; private set; } = null!;
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public Role Role { get; private set; }
    public List<Project> InvolvedProjects { get; private set; } = [];
    public List<Project> CreatedProjects { get; private set; } = [];

    public User(long courseId, string name, string email, string password, Role role) {
        this.CourseId = courseId;
        this.Name = name;
        this.Email = email;
        this.Password = HashPassword(password);
        this.Role = role;
    }

    public User(long userId)  => this.UserId = userId;

    public void Update(long courseId, string name, string email, Role role) {
        this.CourseId = courseId;
        this.Name = name;
        this.Email = email;
        this.Role = role;
    }

    public override bool Equals(object? obj) => obj is User user && this.UserId == user.UserId;

    public override int GetHashCode() => HashCode.Combine(this.UserId);
}