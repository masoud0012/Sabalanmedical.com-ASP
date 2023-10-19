using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SabalanMedical.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class TrackingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductSN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QntyPerPc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessNameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MachineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessNames",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("0a3f8a4c-a841-48c1-91ec-79c20c61b0e8"), " ", "دستگاه جوش التراسونیک" },
                    { new Guid("549e4b0f-6a71-49ba-acfe-2ed9d3f05a20"), " ", "دستگاه سیلر بسته بندی" },
                    { new Guid("860fb267-3b6b-4856-a2d5-bd9159a192e5"), " ", "دستگاه تزریق شماره 3" },
                    { new Guid("8d33ebf3-98c4-4901-b419-b641a950d5d8"), " ", "دستگاه تزریق شماره 1" },
                    { new Guid("a356fa0e-f4d7-430d-a798-778b4242276d"), " ", "دستگاه تزریق شماره 2" },
                    { new Guid("b38c91fb-c95f-4b09-9b00-a38e583cb912"), " ", "دستگاه تزریق شماره 4" },
                    { new Guid("d0eef76f-9a21-435a-afca-8b9921560eac"), " ", "دستگاه برش التراسونیک پارچه" },
                    { new Guid("f6fc6676-6a9a-4885-9ba5-6ec10aec56a9"), " ", "دستگاه تزریق شماره عباسی" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("021cdce4-a3d6-4d67-8b3b-8eb40171b20c"), "مهندس عبدالله پور" },
                    { new Guid("2406efac-433e-4846-b48e-49ee8c60bfc4"), "اپراتور 2 عبداللهی" },
                    { new Guid("334456e1-c5b8-4b6f-9fa5-f8b335758259"), "پژمان" },
                    { new Guid("56402bb9-8f45-4674-a79b-645ce88bfc5a"), "رضا" },
                    { new Guid("62ca96b8-2c15-4ba2-a155-20b8a5a5ee6a"), "مسعود" },
                    { new Guid("6ad68274-014c-4a67-8718-cbdb4a8d9265"), "عباسی" },
                    { new Guid("bbb1c7e9-4c6b-462c-9fcd-9d3c49a2883e"), "اپراتور 1 عباسی" }
                });

            migrationBuilder.InsertData(
                table: "ProcessNames",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("59c7a843-2c0c-4ed4-ad2f-c7dc572dfa42"), "جوش التراسونیک" },
                    { new Guid("803c5a2d-88cb-4899-936d-13da0fdc8d8f"), "بسته بندی" },
                    { new Guid("aa425765-ba1f-4319-a4b2-7313b91f0c5f"), "خرید" },
                    { new Guid("f1ed1c0a-5bc6-4cb2-a4f6-6b2d2faeb232"), "برش پارچه" },
                    { new Guid("fa953959-28bc-4f6b-a902-bfbb958245a7"), "تزریق پلاستیک" }
                });

            migrationBuilder.InsertData(
                table: "ProductCats",
                columns: new[] { "Id", "Category" },
                values: new object[,]
                {
                    { new Guid("06672f4e-bf1d-4ee2-91b1-37da3ac9b57b"), "Semi Final Prodct" },
                    { new Guid("6262fc78-accd-40df-bc47-bf16f1ad87e3"), "FinalProduct" },
                    { new Guid("9f1934aa-3a51-4266-806e-04987bf0f04b"), "RawMaterial" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "ProcessDetails");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "ProcessNames");

            migrationBuilder.DropTable(
                name: "ProductCats");
        }
    }
}
