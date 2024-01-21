using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class specification_type_names_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationTypes_SpecificationTypes_ParentTypeId",
                table: "SpecificationTypes");

            migrationBuilder.RenameColumn(
                name: "ParentTypeId",
                table: "SpecificationTypes",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationTypes_ParentTypeId",
                table: "SpecificationTypes",
                newName: "IX_SpecificationTypes_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationTypes_SpecificationTypes_ParentId",
                table: "SpecificationTypes",
                column: "ParentId",
                principalTable: "SpecificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationTypes_SpecificationTypes_ParentId",
                table: "SpecificationTypes");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "SpecificationTypes",
                newName: "ParentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationTypes_ParentId",
                table: "SpecificationTypes",
                newName: "IX_SpecificationTypes_ParentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationTypes_SpecificationTypes_ParentTypeId",
                table: "SpecificationTypes",
                column: "ParentTypeId",
                principalTable: "SpecificationTypes",
                principalColumn: "Id");
        }
    }
}
