using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartDesk.API.Migrations
{
    /// <inheritdoc />
    public partial class FinalApiModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Usuarios_ColaboradorId",
                table: "Chamados");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Usuarios_TecnicoId",
                table: "Chamados");

            migrationBuilder.AddColumn<int>(
                name: "Especialidade",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prioridade",
                table: "Chamados",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Usuarios_ColaboradorId",
                table: "Chamados",
                column: "ColaboradorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Usuarios_TecnicoId",
                table: "Chamados",
                column: "TecnicoId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Usuarios_ColaboradorId",
                table: "Chamados");

            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Usuarios_TecnicoId",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Chamados");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Usuarios_ColaboradorId",
                table: "Chamados",
                column: "ColaboradorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Usuarios_TecnicoId",
                table: "Chamados",
                column: "TecnicoId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
