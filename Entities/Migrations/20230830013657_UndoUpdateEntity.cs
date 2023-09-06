using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class UndoUpdateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductDesc");

            migrationBuilder.DropTable(
                name: "ProductProductImg");

            migrationBuilder.DropTable(
                name: "ProductProductProperty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductProductDesc",
                columns: table => new
                {
                    DesctiptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductDescDesctiptionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductDesc", x => new { x.DesctiptionID, x.ProductDescDesctiptionID });
                    table.ForeignKey(
                        name: "FK_ProductProductDesc_ProductDescs_ProductDescDesctiptionID",
                        column: x => x.ProductDescDesctiptionID,
                        principalTable: "ProductDescs",
                        principalColumn: "DesctiptionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductDesc_Products_DesctiptionID",
                        column: x => x.DesctiptionID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductImg",
                columns: table => new
                {
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductImgsImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductImg", x => new { x.ImageID, x.ProductImgsImageID });
                    table.ForeignKey(
                        name: "FK_ProductProductImg_ProductImgs_ProductImgsImageID",
                        column: x => x.ProductImgsImageID,
                        principalTable: "ProductImgs",
                        principalColumn: "ImageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductImg_Products_ImageID",
                        column: x => x.ImageID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductProperty",
                columns: table => new
                {
                    ProductPropertypropertyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    propertyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductProperty", x => new { x.ProductPropertypropertyID, x.propertyID });
                    table.ForeignKey(
                        name: "FK_ProductProductProperty_ProductProperties_ProductPropertypropertyID",
                        column: x => x.ProductPropertypropertyID,
                        principalTable: "ProductProperties",
                        principalColumn: "propertyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductProperty_Products_propertyID",
                        column: x => x.propertyID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductDesc_ProductDescDesctiptionID",
                table: "ProductProductDesc",
                column: "ProductDescDesctiptionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductImg_ProductImgsImageID",
                table: "ProductProductImg",
                column: "ProductImgsImageID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductProperty_propertyID",
                table: "ProductProductProperty",
                column: "propertyID");
        }
    }
}
