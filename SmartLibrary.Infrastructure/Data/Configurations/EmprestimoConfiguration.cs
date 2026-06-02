using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
{
    public void Configure(EntityTypeBuilder<Emprestimo> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(e => e.Observacao).HasMaxLength(500);
        builder.Property(e => e.DataEmprestimo).IsRequired();
        builder.Property(e => e.DataPrevistaDevolucao).IsRequired();

        builder.HasOne(e => e.Exemplar)
               .WithMany(ex => ex.Emprestimos)
               .HasForeignKey(e => e.ExemplarId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Aluno)
               .WithMany(a => a.Emprestimos)
               .HasForeignKey(e => e.AlunoId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Bibliotecario)
               .WithMany()
               .HasForeignKey(e => e.BibliotecarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
