using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDesk.API.Models
{
    public class Chamado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Protocolo { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        public StatusChamado Status { get; set; } = StatusChamado.Aberto;

        [StringLength(50)]
        public string Prioridade { get; set; } = "Média"; 

        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;

        public DateTime? DataResolucao { get; set; }

        public DateTime? DataFechamento { get; set; }

        public int ColaboradorId { get; set; }
        public int? TecnicoId { get; set; }

        [ForeignKey("ColaboradorId")]
        public virtual Usuario Colaborador { get; set; } = null!;

        [ForeignKey("TecnicoId")]
        public virtual Usuario? Tecnico { get; set; }

        public virtual ICollection<MensagemChamado> Mensagens { get; set; } = new List<MensagemChamado>();
    }
}