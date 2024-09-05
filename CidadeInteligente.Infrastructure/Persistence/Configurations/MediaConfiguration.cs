using CidadeInteligente.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media> {
    public void Configure(EntityTypeBuilder<Media> builder) {
        builder.HasKey(m => m.MediaId);

        builder.Property(m => m.Title).HasMaxLength(60);
        builder.Property(m => m.Description)
               .IsRequired(false)
               .HasMaxLength(300);

        builder.HasOne(m => m.Project)
               .WithMany(p => p.Medias)
               .HasForeignKey(m => m.ProjectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}