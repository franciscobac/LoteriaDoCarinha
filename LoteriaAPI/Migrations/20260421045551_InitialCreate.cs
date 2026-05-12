using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoteriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loterias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuantidadeNumeros = table.Column<int>(type: "int", nullable: false),
                    MinNumero = table.Column<int>(type: "int", nullable: false),
                    MaxNumero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loterias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Concursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoLoteriaId = table.Column<int>(type: "int", nullable: false),
                    NumeroConcurso = table.Column<int>(type: "int", nullable: false),
                    DataSorteio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumerosSorteados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acumulou = table.Column<bool>(type: "bit", nullable: false),
                    ValorAcumuladoProximo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValorArrecadado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LocalSorteio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MunicipioUFSorteio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Premiacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataConsulta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concursos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Concursos_Loterias_TipoLoteriaId",
                        column: x => x.TipoLoteriaId,
                        principalTable: "Loterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NumerosGerados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    TipoLoteriaId = table.Column<int>(type: "int", nullable: false),
                    Numeros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataGeracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraGeracao = table.Column<TimeSpan>(type: "time", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumerosGerados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NumerosGerados_Loterias_TipoLoteriaId",
                        column: x => x.TipoLoteriaId,
                        principalTable: "Loterias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NumerosGerados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Loterias",
                columns: new[] { "Id", "MaxNumero", "MinNumero", "Nome", "QuantidadeNumeros" },
                values: new object[,]
                {
                    { 1, 60, 1, "MEGA-SENA", 6 },
                    { 2, 25, 1, "LOTOFACIL", 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Concursos_TipoLoteriaId_NumeroConcurso",
                table: "Concursos",
                columns: new[] { "TipoLoteriaId", "NumeroConcurso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NumerosGerados_TipoLoteriaId",
                table: "NumerosGerados",
                column: "TipoLoteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_NumerosGerados_UsuarioId",
                table: "NumerosGerados",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Concursos");

            migrationBuilder.DropTable(
                name: "NumerosGerados");

            migrationBuilder.DropTable(
                name: "Loterias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
