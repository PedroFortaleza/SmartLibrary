using System.Text.Json;
using SmartLibrary.Application.DTOs.External;
using SmartLibrary.Application.Interfaces.External;

namespace SmartLibrary.Infrastructure.HttpClients;

public class OpenLibraryClient(HttpClient httpClient) : IOpenLibraryService
{
    public async Task<GoogleBooksResultDto?> GetByIsbnAsync(string isbn)
    {
        try
        {
            var url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var key = $"ISBN:{isbn}";
            if (!root.TryGetProperty(key, out var book)) return null;

            var autores = new List<string>();
            if (book.TryGetProperty("authors", out var authorsEl))
                foreach (var a in authorsEl.EnumerateArray())
                    if (a.TryGetProperty("name", out var name))
                        autores.Add(name.GetString() ?? "");

            string? capaUrl = null;
            if (book.TryGetProperty("cover", out var cover) && cover.TryGetProperty("medium", out var med))
                capaUrl = med.GetString();

            int? ano = null;
            if (book.TryGetProperty("publish_date", out var pd))
            {
                var dateStr = pd.GetString() ?? "";
                var parts = dateStr.Split(' ');
                foreach (var p in parts)
                    if (int.TryParse(p, out var y) && y > 1000 && y < 2100) { ano = y; break; }
            }

            string? editora = null;
            if (book.TryGetProperty("publishers", out var pubs) && pubs.GetArrayLength() > 0)
                if (pubs[0].TryGetProperty("name", out var pname))
                    editora = pname.GetString();

            return new GoogleBooksResultDto
            {
                Titulo = book.TryGetProperty("title", out var t) ? t.GetString() : null,
                Editora = editora,
                AnoPublicacao = ano,
                CapaUrl = capaUrl,
                Autores = autores
            };
        }
        catch
        {
            return null;
        }
    }
}
