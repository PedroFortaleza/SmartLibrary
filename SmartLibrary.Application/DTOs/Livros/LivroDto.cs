using SmartLibrary.Application.DTOs.Avaliacoes;
using SmartLibrary.Application.DTOs.Exemplares;

namespace SmartLibrary.Application.DTOs.Livros;

public class LivroDto : LivroListDto
{
    public string? Idioma { get; set; }
    public int? NumeroPaginas { get; set; }
    public string? Sinopse { get; set; }
    public string? Edicao { get; set; }
    public DateTime CriadoEm { get; set; }
    public List<ExemplarDto> Exemplares { get; set; } = [];
    public List<AvaliacaoDto> Avaliacoes { get; set; } = [];
}
