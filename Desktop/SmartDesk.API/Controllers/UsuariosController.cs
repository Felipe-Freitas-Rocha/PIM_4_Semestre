using Microsoft.AspNetCore.Mvc;
using SmartDesk.API.Data;
using SmartDesk.API.DTOs;
using SmartDesk.API.Models;
using SmartDesk.API.Services;
using Microsoft.AspNetCore.Authorization; // << Adicione este using
using Microsoft.EntityFrameworkCore;     // << Adicione este using

namespace SmartDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] // << SEGURANÇA: Só Admins podem acessar qualquer função neste controller
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;

        public UsuariosController(ApplicationDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // POST /api/usuarios (Já existente)
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioDto novoUsuarioDto)
        {
            var senhaAleatoria = _passwordService.GenerateRandomPassword();
            var usuario = new Usuario
            {
                NomeCompleto = novoUsuarioDto.NomeCompleto,
                NivelAcesso = novoUsuarioDto.NivelAcesso,
                SenhaHash = _passwordService.HashPassword(senhaAleatoria),
                IsAtivo = true,
                DataCriacao = DateTime.UtcNow
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            var resposta = new
            {
                Message = "Usuário criado com sucesso!",
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                SenhaGerada = senhaAleatoria
            };

            return CreatedAtAction(nameof(CriarUsuario), new { id = usuario.Id }, resposta);
        }

        // ... NOVO ...
        // GET /api/usuarios - Lista todos os usuários
        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        // ... NOVO ...
        // GET /api/usuarios/5 - Busca um usuário específico pelo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            return Ok(usuario);
        }

        // ... NOVO ...
        // PUT /api/usuarios/5 - Atualiza um usuário existente
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] AtualizarUsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            usuario.NomeCompleto = usuarioDto.NomeCompleto;
            usuario.NivelAcesso = usuarioDto.NivelAcesso;

            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content, indicando sucesso na atualização
        }

        // ... NOVO ...
        // DELETE /api/usuarios/5 - Desativa um usuário (não apaga)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DesativarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            usuario.IsAtivo = false; // Apenas mudamos o status
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna 204 No Content
        }
    }
}