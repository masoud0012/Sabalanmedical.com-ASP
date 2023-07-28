using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class LaparascopyGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "TypeId",
                keyValue: new Guid("7ce17d2d-a945-4fdf-b859-70f954cfe029"));

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "TypeId", "TypeNameEN", "TypeNameFr" },
                values: new object[] { new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"), "Laparascopy", "محصولات لاپاراسکوپی" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTypes",
                keyColumn: "TypeId",
                keyValue: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.InsertData(
                table: "ProductTypes",
                columns: new[] { "TypeId", "TypeNameEN", "TypeNameFr" },
                values: new object[] { new Guid("7ce17d2d-a945-4fdf-b859-70f954cfe029"), "Laparascopy", "محصولات لاپاراسکوپی" });
        }
    }
}
