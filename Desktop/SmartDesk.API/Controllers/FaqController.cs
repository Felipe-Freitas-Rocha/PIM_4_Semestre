using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartDesk.API.Data;
using SmartDesk.API.DTOs;
using SmartDesk.API.Models;

namespace SmartDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FaqController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FaqController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /api/faq - Público, qualquer um pode consultar
        [HttpGet]
        [AllowAnonymous] // Permite acesso sem login
        public async Task<IActionResult> GetAllFaqs()
        {
            return Ok(await _context.FAQs.ToListAsync());
        }

        // POST /api/faq - Apenas Admins podem criar
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CreateFaq([FromBody] CriarFaqDto faqDto)
        {
            var faq = new FAQ
            {
                Pergunta = faqDto.Pergunta,
                Resposta = faqDto.Resposta,
                Categoria = faqDto.Categoria
            };

            await _context.FAQs.AddAsync(faq);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllFaqs), new { id = faq.Id }, faq);
        }

        // PUT /api/faq/{id} - Apenas Admins podem atualizar
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateFaq(int id, [FromBody] AtualizarFaqDto faqDto)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return NotFound();

            faq.Pergunta = faqDto.Pergunta;
            faq.Resposta = faqDto.Resposta;
            faq.Categoria = faqDto.Categoria;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteFaq(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return NotFound();

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}