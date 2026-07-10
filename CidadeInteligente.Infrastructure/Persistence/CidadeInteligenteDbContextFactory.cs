using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CidadeInteligente.Infrastructure.Persistence;

public class CidadeInteligenteDbContextFactory : IDesignTimeDbContextFactory<CidadeInteligenteDbContext>
{
    public CidadeInteligenteDbContext CreateDbContext(string[] args)
    {
        string basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "CidadeInteligente.Mvc"
        );

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString("Database");

        DbContextOptions<CidadeInteligenteDbContext> options = new DbContextOptionsBuilder<CidadeInteligenteDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new CidadeInteligenteDbContext(options);
    }
}
