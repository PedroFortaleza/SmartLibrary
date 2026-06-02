using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class LivroCategoriaConfiguration : IEntityTypeConfiguration<LivroCategoria>
{
    public void Configure(EntityTypeBuilder<LivroCategoria> builder)
    {
        builder.HasKey(lc => new { lc.LivroId, lc.CategoriaId });

        builder.HasOne(lc => lc.Livro)
               .WithMany(l => l.LiveCategorias)
               .HasForeignKey(lc => lc.LivroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(lc => lc.Categoria)
               .WithMany(c => c.LiveCategorias)
               .HasForeignKey(lc => lc.CategoriaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
