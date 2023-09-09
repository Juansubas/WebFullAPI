using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class Revert_AgregarNumeroVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumeroVillas_Villas_VilaId",
                table: "NumeroVillas");

            migrationBuilder.DropIndex(
                name: "IX_NumeroVillas_VilaId",
                table: "NumeroVillas");

            migrationBuilder.DropColumn(
                name: "VilaId",
                table: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 9, 9, 2, 29, 21, 825, DateTimeKind.Local).AddTicks(262), new DateTime(2023, 9, 9, 2, 29, 21, 825, DateTimeKind.Local).AddTicks(253) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 9, 9, 2, 29, 21, 825, DateTimeKind.Local).AddTicks(265), new DateTime(2023, 9, 9, 2, 29, 21, 825, DateTimeKind.Local).AddTicks(264) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NumeroVillas_Villas_VillaId",
                table: "NumeroVillas",
                column: "VillaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NumeroVillas_Villas_VillaId",
                table: "NumeroVillas");

            migrationBuilder.DropIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas");

            migrationBuilder.AddColumn<int>(
                name: "VilaId",
                table: "NumeroVillas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 9, 9, 1, 31, 27, 545, DateTimeKind.Local).AddTicks(4971), new DateTime(2023, 9, 9, 1, 31, 27, 545, DateTimeKind.Local).AddTicks(4959) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 9, 9, 1, 31, 27, 545, DateTimeKind.Local).AddTicks(4974), new DateTime(2023, 9, 9, 1, 31, 27, 545, DateTimeKind.Local).AddTicks(4974) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VilaId",
                table: "NumeroVillas",
                column: "VilaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NumeroVillas_Villas_VilaId",
                table: "NumeroVillas",
                column: "VilaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
