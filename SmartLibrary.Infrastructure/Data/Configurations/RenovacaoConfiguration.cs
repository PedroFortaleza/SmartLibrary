using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class RenovacaoConfiguration : IEntityTypeConfiguration<Renovacao>
{
    public void Configure(EntityTypeBuilder<Renovacao> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.DataRenovacao).IsRequired();
        builder.Property(r => r.NovaDataPrevista).IsRequired();

        builder.HasOne(r => r.Emprestimo)
               .WithMany(e => e.Renovacoes)
               .HasForeignKey(r => r.EmprestimoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Usuario)
               .WithMany()
               .HasForeignKey(r => r.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
