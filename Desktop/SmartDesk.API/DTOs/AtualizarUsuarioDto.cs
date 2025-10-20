using SmartDesk.API.Models;

namespace SmartDesk.API.DTOs
{
    public class AtualizarUsuarioDto
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public NivelAcesso NivelAcesso { get; set; }
    }
}