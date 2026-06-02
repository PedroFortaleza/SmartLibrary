using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Matricula).HasMaxLength(20).IsRequired();
        builder.Property(a => a.Curso).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Turno).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(a => a.Cep).HasMaxLength(9);
        builder.Property(a => a.Logradouro).HasMaxLength(200);
        builder.Property(a => a.Cidade).HasMaxLength(100);
        builder.Property(a => a.UF).HasMaxLength(2).IsFixedLength();
        builder.Property(a => a.Telefone).HasMaxLength(20);

        builder.HasIndex(a => a.Matricula).IsUnique();

        builder.HasOne(a => a.Usuario)
               .WithOne(u => u.Aluno)
               .HasForeignKey<Aluno>(a => a.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
