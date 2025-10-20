using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartDesk.API.Data;
using SmartDesk.API.Models; // << IMPORTANTE: Esta é a linha que evita o erro!

namespace SmartDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] // Apenas Admins podem ver relatórios
    public class RelatoriosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RelatoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint: GET /api/relatorios/estatisticas
        [HttpGet("estatisticas")]
        public async Task<IActionResult> GetEstatisticasGerais()
        {
            var estatisticas = new
            {
                TotalUsuarios = await _context.Usuarios.CountAsync(),
                TotalChamados = await _context.Chamados.CountAsync(),
                ChamadosAbertos = await _context.Chamados.CountAsync(c => c.Status == StatusChamado.Aberto),
                ChamadosEmAndamento = await _context.Chamados.CountAsync(c => c.Status == StatusChamado.EmAndamento),
                ChamadosResolvidos = await _context.Chamados.CountAsync(c => c.Status == StatusChamado.Resolvido)
            };
            return Ok(estatisticas);
        }
    }
}