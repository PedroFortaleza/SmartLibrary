using SmartLibrary.Application.DTOs.Categorias;

namespace SmartLibrary.Application.Interfaces.Services;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> GetAllAsync();
}
