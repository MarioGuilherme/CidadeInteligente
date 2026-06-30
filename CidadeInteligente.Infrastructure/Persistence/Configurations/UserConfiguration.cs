using CidadeInteligente.Core.Entities;
using CidadeInteligente.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Name).HasMaxLength(60);
        builder.Property(u => u.Email).HasMaxLength(60);
        builder.Property(u => u.Password).HasMaxLength(255);

        builder.Property(u => u.TokenRecoverPassword).HasMaxLength(156);

        builder.HasOne(u => u.Course);

        builder.HasMany(u => u.CreatedProjects)
               .WithOne(p => p.CreatedBy)
               .HasForeignKey(p => p.CreatedByUserId);

        builder.HasMany(u => u.InvolvedProjects)
               .WithMany(p => p.InvolvedUsers)
               .UsingEntity(
                    "ProjectsUsers",
                    u => u.HasOne(typeof(Project)).WithMany().HasForeignKey("ProjectId"),
                    p => p.HasOne(typeof(User)).WithMany().HasForeignKey("UserId"),
                    k => k.HasKey("UserId", "ProjectId")
               );

        builder.HasMany(u => u.InvolvedProjects)
               .WithMany(p => p.InvolvedUsers);

        builder.HasData([
            new(1, 1, "Usuário de Demonstração", "demo@demo.com", "$2a$12$6Mv0u92cyvPnf7c.2rvdmen5RpawVRPvfIsADYfEx915HDxGeMll.", Role.Teacher)]); // Password: demo
    }
}