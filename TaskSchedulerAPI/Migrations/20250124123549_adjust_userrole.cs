using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskSchedulerAPI.Migrations
{
    /// <inheritdoc />
    public partial class adjustuserrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a3b766e-1bea-4d8b-9f4e-d9862ca45fa7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e34ab40b-fe24-469d-88a3-d8043e441b49");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "176574e8-e688-45ea-9352-aa8e9fc54939", null, "Admin", "ADMIN" },
                    { "dc2cfe55-e9e9-4aab-bb75-dafc40b5b5b8", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "176574e8-e688-45ea-9352-aa8e9fc54939");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc2cfe55-e9e9-4aab-bb75-dafc40b5b5b8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a3b766e-1bea-4d8b-9f4e-d9862ca45fa7", null, "Admin", "ADMIN" },
                    { "e34ab40b-fe24-469d-88a3-d8043e441b49", null, "User", "NAME" }
                });
        }
    }
}
