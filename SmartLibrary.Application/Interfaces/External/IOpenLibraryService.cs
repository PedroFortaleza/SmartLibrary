using SmartLibrary.Application.DTOs.External;

namespace SmartLibrary.Application.Interfaces.External;

public interface IOpenLibraryService
{
    Task<GoogleBooksResultDto?> GetByIsbnAsync(string isbn);
}
