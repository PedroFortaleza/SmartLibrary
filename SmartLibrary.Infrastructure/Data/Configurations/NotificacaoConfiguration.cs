using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class NotificacaoConfiguration : IEntityTypeConfiguration<Notificacao>
{
    public void Configure(EntityTypeBuilder<Notificacao> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Tipo).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(n => n.Mensagem).HasMaxLength(500).IsRequired();
        builder.Property(n => n.Lida).HasDefaultValue(false);
        builder.Property(n => n.CriadaEm).IsRequired();
    }
}
