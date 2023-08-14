using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_AddProduct = @"CREATE PROCEDURE AddProduct
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
                    INSERT INTO Products (ProductID,TypeId,ProductNameEn,ProductNameFr,ProductUrl,isHotSale,isManufactured)
                    VALUES (@ProductID,@TypeId,@ProductNameEn,@ProductNameFr,@ProductUrl,@isHotSale,@isManufactured)
                END
                ";
            migrationBuilder.Sql(sp_AddProduct);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_AddProduct = @"DROP PROCEDURE AddProduct";
            migrationBuilder.Sql(sp_AddProduct);

        }
    }
}
