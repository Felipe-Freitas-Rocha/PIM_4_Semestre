using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartDesk.API.Data;
using SmartDesk.API.DTOs;
using SmartDesk.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SmartDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")] // URL: POST /api/auth/login
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // 1. Busca o usuário pelo ID fornecido
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == loginDto.Id);

            // 2. Verifica se o usuário existe, se a senha corresponde e se ele está ativo
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash) || !usuario.IsAtivo)
            {
                return Unauthorized(new { Message = "ID, senha inválidos ou usuário inativo." });
            }

            // 3. Se as credenciais são válidas, gera o Token JWT
            var token = GenerateJwtToken(usuario);

            // 4. Retorna o token para o cliente
            return Ok(new { Token = token });
        }
        
        // V----- MÉTODO TEMPORÁRIO ADICIONADO -----V
        // ATENÇÃO: Este método é apenas uma ferramenta temporária de desenvolvimento!
        // Deve ser removido antes de publicar o projeto.
        [HttpGet("hash/{senha}")]
        public IActionResult GerarHash(string senha)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(senha);
            return Ok(new { SenhaOriginal = senha, SenhaHasheada = hash });
        }
        // A------------------------------------------A

        private string GenerateJwtToken(Models.Usuario usuario)
        {
            // Pega a chave secreta e outras configurações do appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // As "claims" são as informações que queremos guardar dentro do token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // ID do usuário
                new Claim(ClaimTypes.Name, usuario.NomeCompleto),              // Nome do usuário
                new Claim(ClaimTypes.Role, usuario.NivelAcesso.ToString())     // PERMISSÃO (o mais importante!)
            };

            // Cria o token com as claims, data de expiração e credenciais de assinatura
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8), // Token válido por 8 horas
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}