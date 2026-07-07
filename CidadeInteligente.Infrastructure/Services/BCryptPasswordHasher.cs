using CidadeInteligente.Domain.Services;
using static BCrypt.Net.BCrypt;

namespace CidadeInteligente.Infrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password) => HashPassword(password);

    public bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
