using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.ISBN).HasMaxLength(20).IsRequired();
        builder.Property(l => l.Titulo).HasMaxLength(300).IsRequired();
        builder.Property(l => l.SubTitulo).HasMaxLength(300);
        builder.Property(l => l.Editora).HasMaxLength(150);
        builder.Property(l => l.Edicao).HasMaxLength(20);
        builder.Property(l => l.Idioma).HasMaxLength(50);
        builder.Property(l => l.Sinopse).HasColumnType("text");
        builder.Property(l => l.CapaUrl).HasMaxLength(500);
        builder.Property(l => l.GoogleBooksId).HasMaxLength(50);
        builder.Property(l => l.OpenLibraryId).HasMaxLength(50);
        builder.Property(l => l.CriadoEm).IsRequired();

        builder.HasIndex(l => l.ISBN).IsUnique();
    }
}
