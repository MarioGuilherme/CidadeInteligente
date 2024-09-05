using CidadeInteligente.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.HasKey(u => u.UserId);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Name).HasMaxLength(60);
        builder.Property(u => u.Email).HasMaxLength(60);
        builder.Property(u => u.Password).HasMaxLength(255);

        builder.HasOne(u => u.Course);

        builder.HasMany(u => u.CreatedProjects)
               .WithOne(p => p.CreatorUser)
               .HasForeignKey(p => p.CreatorUserId);

        builder.HasMany(u => u.InvolvedProjects)
               .WithMany(p => p.InvolvedUsers)
               .UsingEntity(
                   "ProjectsUsers",
                   p => p.HasOne(typeof(Project)).WithMany().HasForeignKey("ProjectId").HasPrincipalKey("ProjectId"),
                   u => u.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey("UserId"),
                   k => k.HasKey("UserId", "ProjectId")
               );
    }
}