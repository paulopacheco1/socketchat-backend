using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocketChat.Infrastructure.Persistence.EFCore.Migrations
{
    public partial class AddConversas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConversaUsuario",
                columns: table => new
                {
                    ConversasId = table.Column<int>(type: "int", nullable: false),
                    ParticipantesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversaUsuario", x => new { x.ConversasId, x.ParticipantesId });
                    table.ForeignKey(
                        name: "FK_ConversaUsuario_Conversa_ConversasId",
                        column: x => x.ConversasId,
                        principalTable: "Conversa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversaUsuario_Usuarios_ParticipantesId",
                        column: x => x.ParticipantesId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemetenteId = table.Column<int>(type: "int", nullable: true),
                    ConversaId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensagem_Conversa_ConversaId",
                        column: x => x.ConversaId,
                        principalTable: "Conversa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensagem_Usuarios_RemetenteId",
                        column: x => x.RemetenteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversaUsuario_ParticipantesId",
                table: "ConversaUsuario",
                column: "ParticipantesId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagem_ConversaId",
                table: "Mensagem",
                column: "ConversaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagem_RemetenteId",
                table: "Mensagem",
                column: "RemetenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversaUsuario");

            migrationBuilder.DropTable(
                name: "Mensagem");

            migrationBuilder.DropTable(
                name: "Conversa");
        }
    }
}
