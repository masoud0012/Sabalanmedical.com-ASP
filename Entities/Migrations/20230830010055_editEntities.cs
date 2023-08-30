using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class editEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DesctiptionID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "propertyID",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("01756d04-f7c1-4815-ab5c-0555d9c62b35"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("106f346e-b5e9-4117-b6e1-eb0273a65889"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("398b7e58-bbc3-456f-924e-074c264c4031"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("4ae1d802-87e4-40fe-b148-c624e8520bb1"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("58c1f884-560a-4433-8467-6df7d7779aa8"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("59ec2c5b-e639-40da-b7b8-95a6d31f2d6d"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("5bfa2312-a815-46e3-bb71-38c1d441d69b"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("6488e50c-b8a0-4af8-9d65-71366156b106"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("7d4756e1-f334-45a2-9cf4-b02c8de398bf"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("91db0db1-30b6-4934-8ab6-9bccdb0d4b74"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("94e8c44c-b1e4-43b2-9748-c9ed815e597d"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("96043472-1394-414a-859b-dbae2ee0b9ca"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("9fbdbdfb-ebda-4c01-94b6-37df0b7f4233"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("b21aef73-8ded-4cc3-9aad-adfbd150bed5"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("b380acf2-45d4-40cc-8413-2b0c38666e3b"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("c4821c34-c92c-4a0f-a94b-bfd4ad6cfd8b"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("cadaf485-37c8-43c1-a784-8a1486d4b0d4"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("dd713b1e-379b-43d3-a59e-571c6230ff86"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("e88c63c0-b56f-4749-95c9-b6f081cf667c"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("f2da3a12-5362-4c64-a5cc-3821a6f40502"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("fc72c813-10b4-42f6-a828-9e2b4c6723e4"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductID",
                keyValue: new Guid("fd955430-0230-4f1d-92f5-bcaded597262"),
                columns: new[] { "DesctiptionID", "ImageID", "propertyID" },
                values: new object[] { null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Products_DesctiptionID",
                table: "Products",
                column: "DesctiptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImageID",
                table: "Products",
                column: "ImageID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_propertyID",
                table: "Products",
                column: "propertyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDescs_DesctiptionID",
                table: "Products",
                column: "DesctiptionID",
                principalTable: "ProductDescs",
                principalColumn: "DesctiptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductImgs_ImageID",
                table: "Products",
                column: "ImageID",
                principalTable: "ProductImgs",
                principalColumn: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductProperties_propertyID",
                table: "Products",
                column: "propertyID",
                principalTable: "ProductProperties",
                principalColumn: "propertyID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDescs_DesctiptionID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductImgs_ImageID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductProperties_propertyID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DesctiptionID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImageID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_propertyID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DesctiptionID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "propertyID",
                table: "Products");
        }
    }
}
