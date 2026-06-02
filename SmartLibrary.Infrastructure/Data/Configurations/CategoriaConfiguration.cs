using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Descricao).HasMaxLength(300);

        builder.HasIndex(c => c.Nome).IsUnique();
    }
}
