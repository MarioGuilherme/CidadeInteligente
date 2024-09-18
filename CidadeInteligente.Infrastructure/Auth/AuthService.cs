using CidadeInteligente.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CidadeInteligente.Infrastructure.Auth;

public class AuthService(IConfiguration configuration) : IAuthService {
    private readonly IConfiguration _configuration = configuration;

    public string ComputeSha256Hash(string rawPassword) {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawPassword));

        StringBuilder builder = new();

        for (int i = 0; i < bytes.Length; i++)
            builder.Append(bytes[i].ToString("x2"));

        return builder.ToString();
    }

    public string GenerateJwtToken(string email, string role) {
        string issuer = this._configuration["JWTConfigs:Issuer"]!;
        string audience = this._configuration["JWTConfigs:Audience"]!;
        string key = this._configuration["JWTConfigs:Key"]!;

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [
            new("email", email),
            new(ClaimTypes.Role, role),
        ];

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: credentials,
            claims: claims
        );

        JwtSecurityTokenHandler tokenHandler = new();

        string stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }
}