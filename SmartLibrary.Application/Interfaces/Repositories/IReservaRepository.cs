using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Interfaces.Repositories;

public interface IReservaRepository : IBaseRepository<Reserva>
{
    Task<List<Reserva>> GetPendentesPorLivroAsync(int livroId);
    Task<List<Reserva>> GetByAlunoAsync(int alunoId);
    Task<List<Reserva>> GetAllWithDetailsAsync();
    Task<bool> ExisteReservaPendenteAsync(int livroId, int alunoId);
}
