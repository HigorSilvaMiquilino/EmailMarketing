using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailMarketing.Migrations
{
    /// <inheritdoc />
    public partial class LogsEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogsEnvio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Assunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PromocaoId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsEnvio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsEnvio_Promocoes_PromocaoId",
                        column: x => x.PromocaoId,
                        principalTable: "Promocoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsAbertura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogEnvioId = table.Column<int>(type: "int", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAbertura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsAbertura_LogsEnvio_LogEnvioId",
                        column: x => x.LogEnvioId,
                        principalTable: "LogsEnvio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsClique",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogEnvioId = table.Column<int>(type: "int", nullable: false),
                    DataClique = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsClique", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsClique_LogsEnvio_LogEnvioId",
                        column: x => x.LogEnvioId,
                        principalTable: "LogsEnvio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsErro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogEnvioId = table.Column<int>(type: "int", nullable: false),
                    DataErro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MensagemErro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsErro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsErro_LogsEnvio_LogEnvioId",
                        column: x => x.LogEnvioId,
                        principalTable: "LogsEnvio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAbertura_LogEnvioId",
                table: "LogsAbertura",
                column: "LogEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsClique_LogEnvioId",
                table: "LogsClique",
                column: "LogEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsEnvio_PromocaoId",
                table: "LogsEnvio",
                column: "PromocaoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogsErro_LogEnvioId",
                table: "LogsErro",
                column: "LogEnvioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsAbertura");

            migrationBuilder.DropTable(
                name: "LogsClique");

            migrationBuilder.DropTable(
                name: "LogsErro");

            migrationBuilder.DropTable(
                name: "LogsEnvio");
        }
    }
}
