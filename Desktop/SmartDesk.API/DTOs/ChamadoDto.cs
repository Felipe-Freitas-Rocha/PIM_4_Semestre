namespace SmartDesk.API.DTOs
{
    public class CriarChamadoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }

    public class EscalarChamadoDto
    {
        public int NovoTecnicoId { get; set; }
    }

    // V----- CLASSE FALTANDO ADICIONADA AQUI -----V
    public class ChamadoDetalheDto
    {
        public int Id { get; set; }
        public string Protocolo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Prioridade { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }

        // Dados enriquecidos
        public int ColaboradorId { get; set; }
        public string NomeColaborador { get; set; } = string.Empty;
        public int? TecnicoId { get; set; }
        public string? NomeTecnico { get; set; }
    }
    // A---------------------------------------------A
}