using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class BaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ProductTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "propertyID",
                table: "ProductProperties",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ImageID",
                table: "ProductImgs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DesctiptionID",
                table: "ProductDescs",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductTypes",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductProperties",
                newName: "propertyID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductImgs",
                newName: "ImageID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductDescs",
                newName: "DesctiptionID");
        }
    }
}
