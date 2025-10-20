using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDesk.API.Models
{
    public class MensagemChamado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ChamadoId { get; set; } // Liga a mensagem ao chamado

        [Required]
        public int UsuarioId { get; set; } // Liga a mensagem a quem a enviou

        [Required]
        public string Texto { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

        // Campo para o futuro upload de anexos
        public string? UrlAnexo { get; set; }

        // Propriedades de Navegação (para o EF entender as relações)
        [ForeignKey("ChamadoId")]
        public virtual Chamado Chamado { get; set; } = null!;

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}