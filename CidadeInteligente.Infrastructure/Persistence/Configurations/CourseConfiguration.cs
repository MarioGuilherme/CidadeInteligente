using CidadeInteligente.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course> {
    public void Configure(EntityTypeBuilder<Course> builder) {
        builder.HasKey(c => c.CourseId);

        builder.Property(c => c.Description).HasMaxLength(45);

        builder.HasMany(c => c.Projects)
               .WithOne(p => p.Course)
               .HasForeignKey(p => p.CourseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasData([
            new(1, "Demonstração")
        ]);
    }
}