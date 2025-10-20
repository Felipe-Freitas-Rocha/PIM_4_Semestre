namespace SmartDesk.API.DTOs
{
    public class CriarFaqDto
    {
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }

    public class AtualizarFaqDto
    {
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }
}