using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoices.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ProductsandsectionsFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_SectionId",
                table: "Product",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Section_SectionId",
                table: "Product",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Section_SectionId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_SectionId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Product");
        }
    }
}
