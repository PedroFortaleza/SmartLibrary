using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
{
    public void Configure(EntityTypeBuilder<Reserva> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Property(r => r.DataReserva).IsRequired();
        builder.Property(r => r.DataExpiracao).IsRequired();

        builder.HasOne(r => r.Livro)
               .WithMany(l => l.Reservas)
               .HasForeignKey(r => r.LivroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Aluno)
               .WithMany(a => a.Reservas)
               .HasForeignKey(r => r.AlunoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
