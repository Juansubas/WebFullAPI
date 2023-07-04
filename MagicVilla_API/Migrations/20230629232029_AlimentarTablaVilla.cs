using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Name", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa", new DateTime(2023, 6, 29, 18, 20, 29, 592, DateTimeKind.Local).AddTicks(8696), new DateTime(2023, 6, 29, 18, 20, 29, 592, DateTimeKind.Local).AddTicks(8683), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la villa", new DateTime(2023, 6, 29, 18, 20, 29, 592, DateTimeKind.Local).AddTicks(8699), new DateTime(2023, 6, 29, 18, 20, 29, 592, DateTimeKind.Local).AddTicks(8699), "", 40, "Premium Vista a la Piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
