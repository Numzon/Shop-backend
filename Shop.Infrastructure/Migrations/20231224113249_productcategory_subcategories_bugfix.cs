using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class productcategory_subcategories_bugfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ProductCategoryId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProductCategoryId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductCategoryId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductCategoryId",
                table: "Categories",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ProductCategoryId",
                table: "Categories",
                column: "ProductCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
