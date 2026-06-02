using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface IMultaRepository : IBaseRepository<Multa>
{
    Task<List<Multa>> GetPendentesPorAlunoAsync(int alunoId);
    Task<List<Multa>> GetAllWithDetailsAsync();
    Task<bool> AlunoTemMultaPendenteAsync(int alunoId);
}
