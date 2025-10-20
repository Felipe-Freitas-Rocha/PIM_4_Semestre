using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDesk.API.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        [Required]
        public NivelAcesso NivelAcesso { get; set; }

        public EspecialidadeTecnico? Especialidade { get; set; }

        public bool IsAtivo { get; set; } = true;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Chamado> ChamadosComoColaborador { get; set; } = new List<Chamado>();

        public virtual ICollection<Chamado> ChamadosComoTecnico { get; set; } = new List<Chamado>();
    }
}