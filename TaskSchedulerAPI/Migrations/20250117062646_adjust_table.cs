using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskSchedulerAPI.Migrations
{
    /// <inheritdoc />
    public partial class adjusttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskModel_AspNetUsers_owner_id",
                table: "TaskModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskModel_StatusModel_StatusId",
                table: "TaskModel");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskModel_TaskLevelModel_TaskLevelId",
                table: "TaskModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskModel",
                table: "TaskModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLevelModel",
                table: "TaskLevelModel");

            migrationBuilder.RenameTable(
                name: "TaskModel",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "TaskLevelModel",
                newName: "TaskLevel");

            migrationBuilder.RenameIndex(
                name: "IX_TaskModel_TaskLevelId",
                table: "Tasks",
                newName: "IX_Tasks_TaskLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskModel_StatusId",
                table: "Tasks",
                newName: "IX_Tasks_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskModel_owner_id",
                table: "Tasks",
                newName: "IX_Tasks_owner_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLevel",
                table: "TaskLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_owner_id",
                table: "Tasks",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_StatusModel_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "StatusModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskLevel_TaskLevelId",
                table: "Tasks",
                column: "TaskLevelId",
                principalTable: "TaskLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_owner_id",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_StatusModel_StatusId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskLevel_TaskLevelId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLevel",
                table: "TaskLevel");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TaskModel");

            migrationBuilder.RenameTable(
                name: "TaskLevel",
                newName: "TaskLevelModel");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_TaskLevelId",
                table: "TaskModel",
                newName: "IX_TaskModel_TaskLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_StatusId",
                table: "TaskModel",
                newName: "IX_TaskModel_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_owner_id",
                table: "TaskModel",
                newName: "IX_TaskModel_owner_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskModel",
                table: "TaskModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLevelModel",
                table: "TaskLevelModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModel_AspNetUsers_owner_id",
                table: "TaskModel",
                column: "owner_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModel_StatusModel_StatusId",
                table: "TaskModel",
                column: "StatusId",
                principalTable: "StatusModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModel_TaskLevelModel_TaskLevelId",
                table: "TaskModel",
                column: "TaskLevelId",
                principalTable: "TaskLevelModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
