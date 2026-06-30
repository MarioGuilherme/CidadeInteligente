using CidadeInteligente.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.ProjectId);

        builder.Property(p => p.Title).HasMaxLength(100);
        builder.Property(p => p.Description)
               .IsRequired(false)
               .HasMaxLength(800);
        builder.Property(p => p.RegisteredAt).HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.FinishedAt).IsRequired(false);

        builder.HasOne(p => p.Area)
               .WithMany(a => a.Projects)
               .HasForeignKey(p => p.AreaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Course)
               .WithMany(c => c.Projects)
               .HasForeignKey(p => p.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Medias)
               .WithOne(m => m.Project)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.CreatedBy)
               .WithMany(u => u.CreatedProjects)
               .HasForeignKey(p => p.CreatedByUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.InvolvedUsers)
               .WithMany(u => u.InvolvedProjects)
               .UsingEntity(
                    "ProjectsUsers",
                    u => u.HasOne(typeof(User)).WithMany().HasForeignKey("UserId"),
                    p => p.HasOne(typeof(Project)).WithMany().HasForeignKey("ProjectId"),
                    k => k.HasKey("UserId", "ProjectId")
               );
    }
}