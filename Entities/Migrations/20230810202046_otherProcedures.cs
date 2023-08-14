using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class otherProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllProductTypes = @"CREATE PROCEDURE GetAllProductTypes
            AS BEGIN
            SELECT * FROM ProductTypes
             END";
            migrationBuilder.Sql(sp_GetAllProductTypes);

            string GetallProductImages = @"CREATE PROCEDURE GetAllProductImages
            AS BEGIN
            SELECT * FROM ProductImgs
            END
            ";
            migrationBuilder.Sql(GetallProductImages);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllProductTypes = @"DROP PROCEDURE GetAllProductTypes";
            migrationBuilder.Sql(sp_GetAllProductTypes);
            string sp_GetAllProductImages = @"DROP PROCEDURE GetAllProductImages";
            migrationBuilder.Sql(sp_GetAllProductImages);
        }
    }
}
