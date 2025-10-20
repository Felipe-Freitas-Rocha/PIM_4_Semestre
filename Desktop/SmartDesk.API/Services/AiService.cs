using Microsoft.EntityFrameworkCore;
using SmartDesk.API.Data;
using SmartDesk.API.Models;

namespace SmartDesk.API.Services
{
    public class TriagemResultado
    {
        public string Prioridade { get; set; } = "Média";
        public int? TecnicoId { get; set; }
    }

    public class AiService
    {
        private readonly ApplicationDbContext _context;
        public AiService(ApplicationDbContext context) { _context = context; }

        public async Task<TriagemResultado> RealizarTriagemAsync(Chamado chamado)
        {
            var textoChamado = (chamado.Titulo + " " + chamado.Descricao).ToLower();
            var analise = AnalisarChamado(textoChamado);
            var tecnicoId = await AtribuirTecnicoPorEspecialidade(analise.Categoria);
            return new TriagemResultado { Prioridade = analise.Prioridade, TecnicoId = tecnicoId };
        }

        private (EspecialidadeTecnico Categoria, string Prioridade) AnalisarChamado(string texto)
        {
            // V----- LÓGICA REORDENADA -----V
            // 1. Verifica primeiro os problemas de Rede, que são mais críticos.
            if (texto.Contains("internet") || texto.Contains("conexão") || texto.Contains("wifi") || texto.Contains("wi-fi") || texto.Contains("sem rede"))
            {
                return (EspecialidadeTecnico.Rede, "Alta");
            }
            // 2. Depois, problemas de Hardware que impedem o trabalho.
            if (texto.Contains("impressora") || texto.Contains("mouse") || texto.Contains("teclado") || texto.Contains("monitor") || texto.Contains("não liga"))
            {
                var prioridade = texto.Contains("não liga") || texto.Contains("parou de funcionar") ? "Alta" : "Média";
                return (EspecialidadeTecnico.Hardware, prioridade);
            }
            // 3. Problemas de Acesso.
            if (texto.Contains("senha") || texto.Contains("acesso") || texto.Contains("resetar") || texto.Contains("esqueci"))
            {
                return (EspecialidadeTecnico.Acesso, "Média");
            }
            // 4. Por último, a categoria mais genérica de Software.
            if (texto.Contains("sistema") || texto.Contains("erro") || texto.Contains("aplicativo") || texto.Contains("lento") || texto.Contains("travou"))
            {
                var prioridade = texto.Contains("travou") || texto.Contains("não abre") ? "Alta" : "Média";
                return (EspecialidadeTecnico.Software, prioridade);
            }
            // A------------------------------A

            // Se nenhuma palavra-chave for encontrada, atribui como Geral e Prioridade Baixa
            return (EspecialidadeTecnico.Geral, "Baixa");
        }

        private async Task<int?> AtribuirTecnicoPorEspecialidade(EspecialidadeTecnico especialidade)
        {
            var tecnicosQuery = _context.Usuarios.Where(u => u.NivelAcesso == NivelAcesso.Tecnico && u.IsAtivo);
            var tecnicosQualificados = await tecnicosQuery.Where(u => u.Especialidade == especialidade)
                .Select(u => new { u.Id, ChamadosAbertos = u.ChamadosComoTecnico.Count(c => c.Status == StatusChamado.Aberto || c.Status == StatusChamado.EmAndamento) }).ToListAsync();
            
            if (!tecnicosQualificados.Any())
            {
                tecnicosQualificados = await tecnicosQuery.Where(u => u.Especialidade == EspecialidadeTecnico.Geral)
                .Select(u => new { u.Id, ChamadosAbertos = u.ChamadosComoTecnico.Count(c => c.Status == StatusChamado.Aberto || c.Status == StatusChamado.EmAndamento) }).ToListAsync();
            }

            if (!tecnicosQualificados.Any()) return null;
            return tecnicosQualificados.OrderBy(t => t.ChamadosAbertos).First().Id;
        }
    }
}