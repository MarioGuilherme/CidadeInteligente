using CidadeInteligente.Core.Enums;
using System.Security.Cryptography;

namespace CidadeInteligente.Core.Entities;

public class User
{
    public long UserId { get; private set; }
    public long CourseId { get; set; }
    public Course Course { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public Role Role { get; private set; }
    public string? TokenRecoverPassword { get; private set; }
    public DateTime? TokenRecoverPasswordExpiration { get; private set; }
    public virtual ICollection<Project> InvolvedProjects { get; private set; } = [];
    public virtual ICollection<Project> CreatedProjects { get; private set; } = [];

    public User(long userId, long courseId, string name, string email, string password, Role role)
    {
        UserId = userId;
        CourseId = courseId;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public User(long courseId, string name, string email, string password, Role role)
    {
        CourseId = courseId;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public User(long userId) => UserId = userId;

    public void Update(long courseId, string name, string email, Role role)
    {
        CourseId = courseId;
        Name = name;
        Email = email;
        Role = role;
    }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
        RemovePasswordResetTokenInformation();
    }

    public void RemovePasswordResetTokenInformation()
    {
        TokenRecoverPassword = null;
        TokenRecoverPasswordExpiration = null;
    }

    public void SaveNewTokenToRecoverPassword()
    {
        byte[] randomBytes = new byte[78];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        TokenRecoverPassword = Convert.ToHexStringLower(randomBytes);
        TokenRecoverPasswordExpiration = DateTime.Now.AddMinutes(60);
    }
}
