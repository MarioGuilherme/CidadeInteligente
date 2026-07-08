using CidadeInteligente.Domain.Constants;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserId);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Name).HasMaxLength(UserConstraints.NameMaxLength);
        builder.Property(u => u.Email).HasMaxLength(UserConstraints.EmailMaxLength);
        builder.Property(u => u.Password).HasMaxLength(UserConstraints.PasswordMaxLength);

        builder.Property(u => u.TokenRecoverPassword).HasMaxLength(UserConstraints.TokenRecoverPasswordMaxLength);

        builder.HasOne(u => u.Course)
               .WithMany()
               .HasForeignKey(u => u.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

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

        builder.HasData([
            new(1, 1, "Usuário de Demonstração", "demo@demo.com", "$2a$12$6Mv0u92cyvPnf7c.2rvdmen5RpawVRPvfIsADYfEx915HDxGeMll.", Role.Teacher)]); // Password: demo
    }
}
