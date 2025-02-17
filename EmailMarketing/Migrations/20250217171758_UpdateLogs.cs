using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailMarketing.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogsAbertura_LogsEnvio_LogEnvioId",
                table: "LogsAbertura");

            migrationBuilder.DropForeignKey(
                name: "FK_LogsClique_LogsEnvio_LogEnvioId",
                table: "LogsClique");

            migrationBuilder.DropForeignKey(
                name: "FK_LogsErro_LogsEnvio_LogEnvioId",
                table: "LogsErro");

            migrationBuilder.DropIndex(
                name: "IX_LogsErro_LogEnvioId",
                table: "LogsErro");

            migrationBuilder.DropIndex(
                name: "IX_LogsClique_LogEnvioId",
                table: "LogsClique");

            migrationBuilder.DropIndex(
                name: "IX_LogsAbertura_LogEnvioId",
                table: "LogsAbertura");

            migrationBuilder.DropColumn(
                name: "LogEnvioId",
                table: "LogsErro");

            migrationBuilder.DropColumn(
                name: "LogEnvioId",
                table: "LogsClique");

            migrationBuilder.DropColumn(
                name: "LogEnvioId",
                table: "LogsAbertura");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LogsErro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LogsErro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LogsClique",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LogsAbertura",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "LogsErro");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LogsErro");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "LogsClique");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "LogsAbertura");

            migrationBuilder.AddColumn<int>(
                name: "LogEnvioId",
                table: "LogsErro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LogEnvioId",
                table: "LogsClique",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LogEnvioId",
                table: "LogsAbertura",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LogsErro_LogEnvioId",
                table: "LogsErro",
                column: "LogEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsClique_LogEnvioId",
                table: "LogsClique",
                column: "LogEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAbertura_LogEnvioId",
                table: "LogsAbertura",
                column: "LogEnvioId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogsAbertura_LogsEnvio_LogEnvioId",
                table: "LogsAbertura",
                column: "LogEnvioId",
                principalTable: "LogsEnvio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogsClique_LogsEnvio_LogEnvioId",
                table: "LogsClique",
                column: "LogEnvioId",
                principalTable: "LogsEnvio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogsErro_LogsEnvio_LogEnvioId",
                table: "LogsErro",
                column: "LogEnvioId",
                principalTable: "LogsEnvio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
