namespace SmartDesk.API.DTOs
{
    public class CriarMensagemDto
    {
        public string Texto { get; set; } = string.Empty;
    }
    public class MensagemDetalheDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; }
    }
}