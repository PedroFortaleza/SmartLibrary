using SmartLibrary.Application.DTOs.External;

namespace SmartLibrary.Application.Interfaces.External;

public interface IGoogleBooksService
{
    Task<GoogleBooksResultDto?> GetByIsbnAsync(string isbn);
}
