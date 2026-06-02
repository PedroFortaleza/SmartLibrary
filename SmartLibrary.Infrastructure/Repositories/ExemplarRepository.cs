using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class ExemplarRepository(SmartLibraryDbContext context) : BaseRepository<Exemplar>(context), IExemplarRepository
{
    public async Task<bool> ExisteCodigoAsync(string codigo, int? ignorarId = null)
        => await Context.Exemplares.AnyAsync(e => e.Codigo == codigo && (ignorarId == null || e.Id != ignorarId));
}
