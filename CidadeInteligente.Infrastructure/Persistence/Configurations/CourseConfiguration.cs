using CidadeInteligente.Domain.Constants;
using CidadeInteligente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.CourseId);

        builder.HasIndex(c => c.Description).IsUnique();
        builder.Property(c => c.Description).HasMaxLength(CourseConstraints.DescriptionMaxLength);

        builder.HasMany(c => c.Projects)
               .WithOne(p => p.Course)
               .HasForeignKey(p => p.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData([
            new(1, "Demonstração")]);
    }
}
