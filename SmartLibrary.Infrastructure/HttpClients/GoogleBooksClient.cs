using System.Text.Json;
using SmartLibrary.Application.DTOs.External;
using SmartLibrary.Application.Interfaces.External;

namespace SmartLibrary.Infrastructure.HttpClients;

public class GoogleBooksClient(HttpClient httpClient) : IGoogleBooksService
{
    public async Task<GoogleBooksResultDto?> GetByIsbnAsync(string isbn)
    {
        try
        {
            var url = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}";
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("totalItems", out var totalItems) || totalItems.GetInt32() == 0)
                return null;

            var items = root.GetProperty("items");
            if (items.GetArrayLength() == 0) return null;

            var volumeInfo = items[0].GetProperty("volumeInfo");
            var id = items[0].GetProperty("id").GetString();

            var autores = new List<string>();
            if (volumeInfo.TryGetProperty("authors", out var authorsEl))
                foreach (var a in authorsEl.EnumerateArray())
                    autores.Add(a.GetString() ?? "");

            int? ano = null;
            if (volumeInfo.TryGetProperty("publishedDate", out var dateEl))
            {
                var dateStr = dateEl.GetString() ?? "";
                if (dateStr.Length >= 4 && int.TryParse(dateStr[..4], out var y))
                    ano = y;
            }

            string? capaUrl = null;
            if (volumeInfo.TryGetProperty("imageLinks", out var imgEl) &&
                imgEl.TryGetProperty("thumbnail", out var thumbEl))
                capaUrl = thumbEl.GetString();

            return new GoogleBooksResultDto
            {
                GoogleBooksId = id,
                Titulo = volumeInfo.TryGetProperty("title", out var t) ? t.GetString() : null,
                SubTitulo = volumeInfo.TryGetProperty("subtitle", out var st) ? st.GetString() : null,
                Editora = volumeInfo.TryGetProperty("publisher", out var pub) ? pub.GetString() : null,
                Descricao = volumeInfo.TryGetProperty("description", out var desc) ? desc.GetString() : null,
                Idioma = volumeInfo.TryGetProperty("language", out var lang) ? lang.GetString() : null,
                NumeroPaginas = volumeInfo.TryGetProperty("pageCount", out var pg) ? pg.GetInt32() : null,
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
