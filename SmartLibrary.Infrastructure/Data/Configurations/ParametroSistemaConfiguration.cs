using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class ParametroSistemaConfiguration : IEntityTypeConfiguration<ParametroSistema>
{
    public void Configure(EntityTypeBuilder<ParametroSistema> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Chave).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Valor).HasMaxLength(300).IsRequired();
        builder.Property(p => p.Descricao).HasMaxLength(300);
        builder.Property(p => p.AtualizadoEm).IsRequired();

        builder.HasIndex(p => p.Chave).IsUnique();
    }
}
