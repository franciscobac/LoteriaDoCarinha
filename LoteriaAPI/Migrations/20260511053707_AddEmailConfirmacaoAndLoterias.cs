using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoteriaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailConfirmacaoAndLoterias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoConfirmacao",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CodigoConfirmacaoExpiracao",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmadoEm",
                table: "Usuarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nome",
                value: "MEGA_SENA");

            migrationBuilder.InsertData(
                table: "Loterias",
                columns: new[] { "Id", "MaxNumero", "MinNumero", "Nome", "QuantidadeNumeros" },
                values: new object[,]
                {
                    { 3, 80, 1, "QUINA", 5 },
                    { 4, 100, 1, "LOTOMANIA", 50 },
                    { 5, 50, 1, "DUPLA_SENA", 6 },
                    { 6, 80, 1, "TIMEMANIA", 10 },
                    { 7, 31, 1, "DIADESORTE", 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DeleteData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "CodigoConfirmacao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoConfirmacaoExpiracao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EmailConfirmado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EmailConfirmadoEm",
                table: "Usuarios");

            migrationBuilder.UpdateData(
                table: "Loterias",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nome",
                value: "MEGA-SENA");
        }
    }
}
