using System.ComponentModel.DataAnnotations;

namespace SmartDesk.API.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Pergunta { get; set; } = string.Empty;

        [Required]
        public string Resposta { get; set; } = string.Empty;

        [StringLength(100)]
        public string Categoria { get; set; } = string.Empty;
    }
}