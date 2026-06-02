using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class AvaliacaoConfiguration : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nota).IsRequired();
        builder.Property(a => a.Comentario).HasColumnType("text");
        builder.Property(a => a.CriadaEm).IsRequired();
        builder.Property(a => a.Aprovada).HasDefaultValue(false);

        builder.HasIndex(a => new { a.LivroId, a.AlunoId }).IsUnique();

        builder.HasOne(a => a.Livro)
               .WithMany(l => l.Avaliacoes)
               .HasForeignKey(a => a.LivroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Aluno)
               .WithMany(a => a.Avaliacoes)
               .HasForeignKey(a => a.AlunoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
