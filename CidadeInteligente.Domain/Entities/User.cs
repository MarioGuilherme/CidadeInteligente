using CidadeInteligente.Domain.Enums;
using System.Security.Cryptography;

namespace CidadeInteligente.Domain.Entities;

public class User
{
    public int UserId { get; private set; }
    public int CourseId { get; private set; }
    public Course Course { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public Role Role { get; private set; }
    public string? TokenRecoverPassword { get; private set; }
    public DateTime? TokenRecoverPasswordExpiration { get; private set; }
    public virtual ICollection<Project> InvolvedProjects { get; private set; } = [];
    public virtual ICollection<Project> CreatedProjects { get; private set; } = [];


    public User(int userId, int courseId, string name, string email, string password, Role role)
    {
        UserId = userId;
        CourseId = courseId;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public User(int courseId, string name, string email, string password, Role role)
    {
        CourseId = courseId;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    public User(int userId) => UserId = userId;

    public void Update(int courseId, string name, string email, Role role)
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
