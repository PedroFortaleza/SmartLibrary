using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class LivroAutorConfiguration : IEntityTypeConfiguration<LivroAutor>
{
    public void Configure(EntityTypeBuilder<LivroAutor> builder)
    {
        builder.HasKey(la => new { la.LivroId, la.AutorId });

        builder.Property(la => la.Ordem).HasDefaultValue(1);

        builder.HasOne(la => la.Livro)
               .WithMany(l => l.LivroAutores)
               .HasForeignKey(la => la.LivroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(la => la.Autor)
               .WithMany(a => a.LivroAutores)
               .HasForeignKey(la => la.AutorId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
