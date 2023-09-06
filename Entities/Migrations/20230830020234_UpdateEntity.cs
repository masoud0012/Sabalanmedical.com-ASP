using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductProperties_ProductID",
                table: "ProductProperties",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImgs_ProductID",
                table: "ProductImgs",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDescs_ProductID",
                table: "ProductDescs",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDescs_Products_ProductID",
                table: "ProductDescs",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImgs_Products_ProductID",
                table: "ProductImgs",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProperties_Products_ProductID",
                table: "ProductProperties",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDescs_Products_ProductID",
                table: "ProductDescs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImgs_Products_ProductID",
                table: "ProductImgs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProperties_Products_ProductID",
                table: "ProductProperties");

            migrationBuilder.DropIndex(
                name: "IX_ProductProperties_ProductID",
                table: "ProductProperties");

            migrationBuilder.DropIndex(
                name: "IX_ProductImgs_ProductID",
                table: "ProductImgs");

            migrationBuilder.DropIndex(
                name: "IX_ProductDescs_ProductID",
                table: "ProductDescs");
        }
    }
}
