using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class ExemplarConfiguration : IEntityTypeConfiguration<Exemplar>
{
    public void Configure(EntityTypeBuilder<Exemplar> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Codigo).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Localizacao).HasMaxLength(100);
        builder.Property(e => e.Tipo).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(e => e.Estado).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(e => e.Ativo).HasDefaultValue(true);
        builder.Property(e => e.CriadoEm).IsRequired();

        builder.HasIndex(e => e.Codigo).IsUnique();

        builder.HasOne(e => e.Livro)
               .WithMany(l => l.Exemplares)
               .HasForeignKey(e => e.LivroId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
