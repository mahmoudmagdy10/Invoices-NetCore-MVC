using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoices.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UserAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8ce947f1-8bba-4d20-8c3c-169356aee4ef", "d25e96e3-66c1-4203-9640-263348e697f2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsAgree", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3789b8d2-8ccc-49da-a2cb-8cda874d4d0e", 0, "c963db2c-6c5a-444d-bfb9-c710e3044dc2", "Mego@gmail.com", true, false, false, null, "MEGO@GMAIL.COM", "MAHMOUDMAGDY", "AQAAAAEAACcQAAAAEP6LeCVbAksRYetN+846j1YMYhAhEnkzjku+QzB9SFEqBTT9PmGLJk1blYwH+oEvkA==", null, false, "", false, "MahmoudMagdy" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "8ce947f1-8bba-4d20-8c3c-169356aee4ef", "3789b8d2-8ccc-49da-a2cb-8cda874d4d0e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8ce947f1-8bba-4d20-8c3c-169356aee4ef", "3789b8d2-8ccc-49da-a2cb-8cda874d4d0e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ce947f1-8bba-4d20-8c3c-169356aee4ef");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3789b8d2-8ccc-49da-a2cb-8cda874d4d0e");
        }
    }
}
