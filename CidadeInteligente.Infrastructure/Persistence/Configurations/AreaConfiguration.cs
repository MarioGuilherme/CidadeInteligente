using CidadeInteligente.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area> {
    public void Configure(EntityTypeBuilder<Area> builder) {
        builder.HasKey(a => a.AreaId);

        builder.Property(a => a.Description).HasMaxLength(45);

        builder.HasMany(a => a.Projects)
               .WithOne(p => p.Area)
               .HasForeignKey(p => p.AreaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}