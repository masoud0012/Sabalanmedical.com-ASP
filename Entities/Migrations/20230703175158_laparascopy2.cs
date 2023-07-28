using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class laparascopy2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("01756d04-f7c1-4815-ab5c-0555d9c62b35"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("106f346e-b5e9-4117-b6e1-eb0273a65889"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("4ae1d802-87e4-40fe-b148-c624e8520bb1"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("58c1f884-560a-4433-8467-6df7d7779aa8"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("59ec2c5b-e639-40da-b7b8-95a6d31f2d6d"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("6488e50c-b8a0-4af8-9d65-71366156b106"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("94e8c44c-b1e4-43b2-9748-c9ed815e597d"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("b380acf2-45d4-40cc-8413-2b0c38666e3b"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c4821c34-c92c-4a0f-a94b-bfd4ad6cfd8b"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("dd713b1e-379b-43d3-a59e-571c6230ff86"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("e88c63c0-b56f-4749-95c9-b6f081cf667c"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("fc72c813-10b4-42f6-a828-9e2b4c6723e4"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("fd955430-0230-4f1d-92f5-bcaded597262"),
                column: "TypeId",
                value: new Guid("15f8dfc3-5c88-4a9e-be20-3ac476192583"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("01756d04-f7c1-4815-ab5c-0555d9c62b35"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("106f346e-b5e9-4117-b6e1-eb0273a65889"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("4ae1d802-87e4-40fe-b148-c624e8520bb1"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("58c1f884-560a-4433-8467-6df7d7779aa8"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("59ec2c5b-e639-40da-b7b8-95a6d31f2d6d"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("6488e50c-b8a0-4af8-9d65-71366156b106"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("94e8c44c-b1e4-43b2-9748-c9ed815e597d"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("b380acf2-45d4-40cc-8413-2b0c38666e3b"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c4821c34-c92c-4a0f-a94b-bfd4ad6cfd8b"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("dd713b1e-379b-43d3-a59e-571c6230ff86"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("e88c63c0-b56f-4749-95c9-b6f081cf667c"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("fc72c813-10b4-42f6-a828-9e2b4c6723e4"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("fd955430-0230-4f1d-92f5-bcaded597262"),
                column: "TypeId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
