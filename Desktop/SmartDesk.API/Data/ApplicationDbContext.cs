using Microsoft.EntityFrameworkCore;
using SmartDesk.API.Models;

namespace SmartDesk.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<MensagemChamado> MensagensChamado { get; set; }
        

        // V----- ADICIONE TODO ESTE MÉTODO ABAIXO -----V
        // Este método é chamado pelo Entity Framework para que possamos
        // configurar manualmente as regras e relações do banco de dados.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relação 1: Um Chamado TEM UM Colaborador, e um Colaborador TEM MUITOS Chamados
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Colaborador) // Lado "um" da relação
                .WithMany(u => u.ChamadosComoColaborador) // Lado "muitos" da relação
                .HasForeignKey(c => c.ColaboradorId) // Chave estrangeira que conecta os dois
                .OnDelete(DeleteBehavior.Restrict); // Impede que um usuário seja deletado se ele tiver chamados

            // Relação 2: Um Chamado TEM UM Técnico (ou nenhum), e um Técnico TEM MUITOS Chamados
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Tecnico) // Lado "um" da relação
                .WithMany(u => u.ChamadosComoTecnico) // Lado "muitos" da relação
                .HasForeignKey(c => c.TecnicoId) // Chave estrangeira que conecta os dois
                .OnDelete(DeleteBehavior.Restrict); // Impede que um técnico seja deletado se ele tiver chamados
        }

    }
}