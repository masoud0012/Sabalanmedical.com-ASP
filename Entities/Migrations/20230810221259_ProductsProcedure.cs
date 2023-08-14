using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class ProductsProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetProductById = @"
                CREATE PROCEDURE GetProductById
                (@productId UNIQUEIDENTIFIER)
                AS BEGIN
                SELECT * FROM Products where ProductId=@productId
                END
                ";
            migrationBuilder.Sql(sp_GetProductById);

            string sp_UpdateProduct = @"
                CREATE PROCEDURE UpdateProduct
            (
                @ProductID      UNIQUEIDENTIFIER,
                @TypeId         UNIQUEIDENTIFIER,
                @ProductNameEn  NVARCHAR (100)  ,
                @ProductNameFr  NVARCHAR (200)  ,
                @ProductUrl     NVARCHAR (200)  ,
                @isHotSale      BIT             ,
                @isManufactured BIT
            )
                AS BEGIN
                UPDATE Products set typeId=@TypeId,ProductNameEn=@ProductNameEn,
                ProductNameFr=@ProductNameFr,ProductUrl=@ProductUrl,isHotSale=@isHotSale,
                isManufactured=@isManufactured where ProductId=@ProductId
                END
                ";
            migrationBuilder.Sql(sp_UpdateProduct);

            string sp_GetProductByProductUrl= @"CREATE PROCEDURE GetProductByProductUrl
            (
            @ProductUrl NVARCHAR (200)
            )
            AS BEGIN
            SELECT * FROM Products where ProductUrl=@ProductUrl
            END
            ";
            migrationBuilder.Sql(sp_GetProductByProductUrl);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetProductById = @"CREATE PROCEDURE GetProductById";
            migrationBuilder.Sql(sp_GetProductById);

            string sp_UpdateProduct = @"DROP PROCEDURE UpdateProduct";
            migrationBuilder.Sql(sp_UpdateProduct);

            string sp_GetProductByProductUrl = @"DROP PROCEDURE GetProductByProductUrl";
            migrationBuilder.Sql(sp_GetProductByProductUrl);
        }
    }
}

