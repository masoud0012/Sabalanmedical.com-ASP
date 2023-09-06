using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllProducts = @"CREATE PROCEDURE GetAllProducts
            AS BEGIN
            SELECT * FROM Products
            END
            ";
            migrationBuilder.Sql(sp_GetAllProducts);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllProducts = @"DROP PROCEDURE GetAllProducts";
            migrationBuilder.Sql(sp_GetAllProducts);

        }
    }
}
