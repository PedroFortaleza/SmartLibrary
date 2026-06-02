using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class AutorConfiguration : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Nacionalidade).HasMaxLength(100);
        builder.Property(a => a.Biografia).HasColumnType("text");
    }
}
