using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class categories_specification_pattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpecificationPatterns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationPatterns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationTypes_SpecificationTypes_ParentTypeId",
                        column: x => x.ParentTypeId,
                        principalTable: "SpecificationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SpecificationPatternId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categories_SpecificationPatterns_SpecificationPatternId",
                        column: x => x.SpecificationPatternId,
                        principalTable: "SpecificationPatterns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecificationPatternSpecificationTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecificationPatternId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecificationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationPatternSpecificationTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationPatternSpecificationTypes_SpecificationPatterns_SpecificationPatternId",
                        column: x => x.SpecificationPatternId,
                        principalTable: "SpecificationPatterns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecificationPatternSpecificationTypes_SpecificationTypes_SpecificationTypeId",
                        column: x => x.SpecificationTypeId,
                        principalTable: "SpecificationTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductCategoryId",
                table: "Categories",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SpecificationPatternId",
                table: "Categories",
                column: "SpecificationPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationPatternSpecificationTypes_SpecificationPatternId",
                table: "SpecificationPatternSpecificationTypes",
                column: "SpecificationPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationPatternSpecificationTypes_SpecificationTypeId",
                table: "SpecificationPatternSpecificationTypes",
                column: "SpecificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationTypes_ParentTypeId",
                table: "SpecificationTypes",
                column: "ParentTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SpecificationPatternSpecificationTypes");

            migrationBuilder.DropTable(
                name: "SpecificationPatterns");

            migrationBuilder.DropTable(
                name: "SpecificationTypes");
        }
    }
}
