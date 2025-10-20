using SmartDesk.API.Models; // Precisamos disso para usar o NivelAcesso

namespace SmartDesk.API.DTOs
{
    public class CriarUsuarioDto
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public NivelAcesso NivelAcesso { get; set; }
    }
}