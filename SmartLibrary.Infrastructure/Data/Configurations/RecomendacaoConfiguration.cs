using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class RecomendacaoConfiguration : IEntityTypeConfiguration<Recomendacao>
{
    public void Configure(EntityTypeBuilder<Recomendacao> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Score).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(r => r.Motivo).HasMaxLength(300);
        builder.Property(r => r.Visualizada).HasDefaultValue(false);
        builder.Property(r => r.CriadaEm).IsRequired();

        builder.HasOne(r => r.Aluno)
               .WithMany(a => a.Recomendacoes)
               .HasForeignKey(r => r.AlunoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Livro)
               .WithMany(l => l.Recomendacoes)
               .HasForeignKey(r => r.LivroId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
