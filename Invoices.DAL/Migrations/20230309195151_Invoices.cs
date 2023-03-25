using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoices.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Invoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "InvoicesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "InvoiceAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesDetails_InvoiceId",
                table: "InvoicesDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAttachments_InvoiceId",
                table: "InvoiceAttachments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ProductId",
                table: "Invoice",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Product_ProductId",
                table: "Invoice",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceAttachments_Invoice_InvoiceId",
                table: "InvoiceAttachments",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesDetails_Invoice_InvoiceId",
                table: "InvoicesDetails",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Product_ProductId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceAttachments_Invoice_InvoiceId",
                table: "InvoiceAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesDetails_Invoice_InvoiceId",
                table: "InvoicesDetails");

            migrationBuilder.DropIndex(
                name: "IX_InvoicesDetails_InvoiceId",
                table: "InvoicesDetails");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceAttachments_InvoiceId",
                table: "InvoiceAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ProductId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "InvoicesDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "InvoiceAttachments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Invoice");
        }
    }
}
