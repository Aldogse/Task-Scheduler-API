using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSchedulerAPI.Migrations
{
    /// <inheritdoc />
    public partial class tasklevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_StatusModels_StatusId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusModels",
                table: "StatusModels");

            migrationBuilder.RenameTable(
                name: "StatusModels",
                newName: "StatusModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusModel",
                table: "StatusModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_StatusModel_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "StatusModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_StatusModel_StatusId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusModel",
                table: "StatusModel");

            migrationBuilder.RenameTable(
                name: "StatusModel",
                newName: "StatusModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusModels",
                table: "StatusModels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_StatusModels_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "StatusModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
