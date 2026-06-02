using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome).HasMaxLength(150).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(200).IsRequired();
        builder.Property(u => u.SenhaHash).HasMaxLength(512).IsRequired();
        builder.Property(u => u.Role).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(u => u.Ativo).HasDefaultValue(true);
        builder.Property(u => u.CriadoEm).IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.Notificacoes)
               .WithOne(n => n.Usuario)
               .HasForeignKey(n => n.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.LogAcoes)
               .WithOne(l => l.Usuario)
               .HasForeignKey(l => l.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
