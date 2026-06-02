using System.Text.Json.Serialization;

namespace SmartLibrary.Application.DTOs.External;

public class ViaCepDto
{
    [JsonPropertyName("logradouro")] public string? Logradouro { get; set; }
    [JsonPropertyName("bairro")]     public string? Bairro { get; set; }
    [JsonPropertyName("localidade")] public string? Localidade { get; set; }
    [JsonPropertyName("uf")]         public string? Uf { get; set; }
    [JsonPropertyName("cep")]        public string? Cep { get; set; }
    [JsonPropertyName("erro")]       public bool Erro { get; set; }
}
