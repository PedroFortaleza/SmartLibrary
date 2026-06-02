using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface IExemplarRepository : IBaseRepository<Exemplar>
{
    Task<bool> ExisteCodigoAsync(string codigo, int? ignorarId = null);
}
