using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infrastructure.Data.Configurations;

public class MultaConfiguration : IEntityTypeConfiguration<Multa>
{
    public void Configure(EntityTypeBuilder<Multa> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.ValorDiario).HasColumnType("decimal(8,2)").IsRequired();
        builder.Property(m => m.ValorTotal).HasColumnType("decimal(10,2)").IsRequired();
        builder.Property(m => m.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
        builder.Property(m => m.FormaPagamento).HasConversion<string>().HasMaxLength(50);

        builder.HasOne(m => m.Emprestimo)
               .WithOne(e => e.Multa)
               .HasForeignKey<Multa>(m => m.EmprestimoId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
