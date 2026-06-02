namespace SmartLibrary.Application.DTOs.Relatorios;

public class RelatorioAcervoDto
{
    public int TotalLivros { get; set; }
    public int TotalExemplares { get; set; }
    public int ExemplaresDisponiveis { get; set; }
    public int ExemplaresEmprestados { get; set; }
    public int ExemplaresReservados { get; set; }
    public int TotalEmprestimosAtivos { get; set; }
    public List<CategoriaAcervoDto> PorCategoria { get; set; } = [];
}

public class CategoriaAcervoDto
{
    public string Categoria { get; set; } = string.Empty;
    public int TotalLivros { get; set; }
    public int TotalExemplares { get; set; }
}

public class RelatorioMultasDto
{
    public int TotalMultas { get; set; }
    public decimal ValorTotal { get; set; }
    public int Pendentes { get; set; }
    public decimal ValorPendente { get; set; }
    public int Pagas { get; set; }
    public decimal ValorPago { get; set; }
    public int Isentas { get; set; }
}
