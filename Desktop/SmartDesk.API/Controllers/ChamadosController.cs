using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartDesk.API.Data;
using SmartDesk.API.DTOs; // << ESTA É A LINHA CRUCIAL QUE ESTAVA FALTANDO
using SmartDesk.API.Models;
using SmartDesk.API.Services;
using System.Security.Claims;

namespace SmartDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Todos os métodos aqui exigem login
    public class ChamadosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AiService _aiService;

        public ChamadosController(ApplicationDbContext context, AiService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        // POST /api/chamados
        [HttpPost]
        [Authorize(Roles = "Colaborador")]
        public async Task<IActionResult> CriarChamado([FromBody] CriarChamadoDto chamadoDto)
        {
            var colaboradorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var anoAtual = DateTime.UtcNow.Year;
            var proximoNumero = (await _context.Chamados.CountAsync(c => c.DataAbertura.Year == anoAtual)) + 1;
            var protocolo = $"TKT-{anoAtual}-{proximoNumero:D4}";

            var chamadoParaAnalise = new Chamado { Titulo = chamadoDto.Titulo, Descricao = chamadoDto.Descricao };
            var resultadoTriagem = await _aiService.RealizarTriagemAsync(chamadoParaAnalise);
            
            var chamadoFinal = new Chamado
            {
                Protocolo = protocolo,
                Titulo = chamadoDto.Titulo,
                Descricao = chamadoDto.Descricao,
                ColaboradorId = colaboradorId,
                Status = StatusChamado.Aberto,
                Prioridade = resultadoTriagem.Prioridade,
                TecnicoId = resultadoTriagem.TecnicoId
            };

            await _context.Chamados.AddAsync(chamadoFinal);
            await _context.SaveChangesAsync();
            
            return Ok(chamadoFinal);
        }

        // GET /api/chamados
        [HttpGet]
        public async Task<IActionResult> ListarChamados()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirstValue(ClaimTypes.Role)!;

            IQueryable<Chamado> query = _context.Chamados
                                        .Include(c => c.Colaborador)
                                        .Include(c => c.Tecnico);

            switch (userRole)
            {
                case "Tecnico":
                    query = query.Where(c => c.TecnicoId == userId);
                    break;
                case "Colaborador":
                    query = query.Where(c => c.ColaboradorId == userId);
                    break;
            }

            var chamadosDto = await query
                .OrderByDescending(c => c.DataAbertura)
                .Select(c => new ChamadoDetalheDto // Esta linha agora funciona
                {
                    Id = c.Id,
                    Protocolo = c.Protocolo,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Status = c.Status.ToString(),
                    Prioridade = c.Prioridade,
                    DataAbertura = c.DataAbertura,
                    ColaboradorId = c.ColaboradorId,
                    NomeColaborador = c.Colaborador.NomeCompleto,
                    TecnicoId = c.TecnicoId,
                    NomeTecnico = c.Tecnico != null ? c.Tecnico.NomeCompleto : "Não atribuído"
                })
                .ToListAsync();

            return Ok(chamadosDto);
        }

        // GET /api/chamados/5 - Busca os detalhes de um único chamado
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChamadoPorId(int id)
        {
            var chamado = await _context.Chamados
                .Include(c => c.Colaborador)
                .Include(c => c.Tecnico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chamado == null)
            {
                return NotFound(new { Message = "Chamado não encontrado." });
            }

            // Validação de segurança
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirstValue(ClaimTypes.Role)!;
            if (userRole == "Colaborador" && chamado.ColaboradorId != userId) return Forbid();
            if (userRole == "Tecnico" && chamado.TecnicoId != userId) return Forbid();

            // Mapeia para o DTO de resposta
            var chamadoDto = new ChamadoDetalheDto // E esta também
            {
                Id = chamado.Id,
                Protocolo = chamado.Protocolo,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                Status = chamado.Status.ToString(),
                Prioridade = chamado.Prioridade,
                DataAbertura = chamado.DataAbertura,
                ColaboradorId = chamado.ColaboradorId,
                NomeColaborador = chamado.Colaborador.NomeCompleto,
                TecnicoId = chamado.TecnicoId,
                NomeTecnico = chamado.Tecnico != null ? chamado.Tecnico.NomeCompleto : "Não atribuído"
            };

            return Ok(chamadoDto);
        }
        
        // GET /api/chamados/{id}/mensagens
        [HttpGet("{id}/mensagens")]
        public async Task<IActionResult> GetMensagensDoChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (User.IsInRole("Colaborador") && chamado.ColaboradorId != userId) return Forbid();
            if (User.IsInRole("Tecnico") && chamado.TecnicoId != userId) return Forbid();

            var mensagens = await _context.MensagensChamado
                .Where(m => m.ChamadoId == id)
                .Include(m => m.Usuario)
                .OrderBy(m => m.DataEnvio)
                .Select(m => new MensagemDetalheDto
                {
                    Id = m.Id,
                    UsuarioId = m.UsuarioId,
                    NomeUsuario = m.Usuario.NomeCompleto,
                    Texto = m.Texto,
                    DataEnvio = m.DataEnvio
                })
                .ToListAsync();

            return Ok(mensagens);
        }

        // POST /api/chamados/{id}/mensagens
        [HttpPost("{id}/mensagens")]
        public async Task<IActionResult> PostarMensagem(int id, [FromBody] CriarMensagemDto mensagemDto)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null) return NotFound();

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (User.IsInRole("Colaborador") && chamado.ColaboradorId != userId) return Forbid();
            if (User.IsInRole("Tecnico") && chamado.TecnicoId != userId) return Forbid();

            var mensagem = new MensagemChamado
            {
                ChamadoId = id,
                UsuarioId = userId,
                Texto = mensagemDto.Texto,
                DataEnvio = DateTime.UtcNow
            };

            await _context.MensagensChamado.AddAsync(mensagem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT /api/chamados/{id}/resolver
        [HttpPut("{id}/resolver")]
        [Authorize(Roles = "Tecnico, Administrador")]
        public async Task<IActionResult> MarcarComoResolvido(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null) return NotFound();
            chamado.Status = StatusChamado.Resolvido;
            chamado.DataResolucao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT /api/chamados/{id}/fechar
        [HttpPut("{id}/fechar")]
        [Authorize(Roles = "Colaborador, Administrador")]
        public async Task<IActionResult> FecharChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null) return NotFound();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (User.IsInRole("Colaborador") && chamado.ColaboradorId != userId) return Forbid();
            chamado.Status = StatusChamado.Fechado;
            chamado.DataFechamento = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT /api/chamados/{id}/escalar
        [HttpPut("{id}/escalar")]
        [Authorize(Roles = "Tecnico, Administrador")]
        public async Task<IActionResult> EscalarChamado(int id, [FromBody] EscalarChamadoDto escalarDto)
        {
            var chamado = await _context.Chamados.FindAsync(id);
            if (chamado == null) return NotFound();
            chamado.TecnicoId = escalarDto.NovoTecnicoId;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}