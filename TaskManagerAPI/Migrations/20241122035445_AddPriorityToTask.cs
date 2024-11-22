using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskManagerAPI.Migrations
{
    public partial class AddPriorityToTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            "ALTER TABLE Tarefas " +
            "ALTER COLUMN Status TYPE integer USING " +
            "(CASE WHEN Status = 'Tarefas' THEN 0 WHEN Status = 'Concluída' THEN 1 ELSE 2 END);");

            migrationBuilder.Sql(
            "ALTER TABLE TaskDto " +
            "ALTER COLUMN Status TYPE integer USING " +
            "(CASE WHEN Status = 'Tarefas' THEN 0 WHEN Status = 'Concluída' THEN 1 ELSE 2 END);");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TaskDto");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Tarefas");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "TaskDto",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Tarefas",
                newName: "Priority");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "TaskDto",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TaskDto",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "TaskDto",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TaskDto",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<List<string>>(
                name: "Categories",
                table: "TaskDto",
                type: "text[]",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Tarefas",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<List<string>>(
                name: "Categories",
                table: "Tarefas",
                type: "text[]",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "TaskDto");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Tarefas");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "TaskDto",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Tarefas",
                newName: "Category");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "TaskDto",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TaskDto",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "TaskDto",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TaskDto",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TaskDto",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tarefas",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Tarefas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
