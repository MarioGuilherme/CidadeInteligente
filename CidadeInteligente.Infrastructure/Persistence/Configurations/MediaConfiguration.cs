using CidadeInteligente.Domain.Constants;
using CidadeInteligente.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CidadeInteligente.Infrastructure.Persistence.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.HasKey(m => m.MediaId);

        builder.Property(m => m.Title).HasMaxLength(MediaConstraints.TitleMaxLength);
        builder.Property(m => m.FileName).HasMaxLength(MediaConstraints.FileNameMaxLength);
        builder.Property(m => m.MimeType).HasMaxLength(MediaConstraints.MimeTypeMaxLength);
        builder.Property(m => m.Description)
               .IsRequired(false)
               .HasMaxLength(MediaConstraints.DescriptionMaxLength);

        builder.HasOne(m => m.Project)
               .WithMany(p => p.Medias)
               .HasForeignKey(m => m.ProjectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
