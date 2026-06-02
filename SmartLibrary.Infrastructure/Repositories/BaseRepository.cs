using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class BaseRepository<T>(SmartLibraryDbContext context) : IBaseRepository<T> where T : class
{
    protected readonly SmartLibraryDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}
