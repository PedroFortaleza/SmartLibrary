using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class LogAcaoConfiguration : IEntityTypeConfiguration<LogAcao>
{
    public void Configure(EntityTypeBuilder<LogAcao> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Acao).HasMaxLength(100).IsRequired();
        builder.Property(l => l.Entidade).HasMaxLength(50);
        builder.Property(l => l.Detalhe).HasColumnType("text");
        builder.Property(l => l.CriadoEm).IsRequired();
    }
}
