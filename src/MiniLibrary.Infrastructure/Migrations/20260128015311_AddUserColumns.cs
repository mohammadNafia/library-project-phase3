using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Authors",
                newName: "DateOfBirth");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Loans",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(@"
                ALTER TABLE ""Books"" 
                ALTER COLUMN ""Genre"" TYPE integer 
                USING CASE 
                    WHEN ""Genre"" = 'Novel' THEN 1
                    WHEN ""Genre"" = 'ShortStory' THEN 2
                    WHEN ""Genre"" = 'Essay' THEN 3
                    WHEN ""Genre"" = 'Poetry' THEN 4
                    WHEN ""Genre"" = 'ResearchPaper' THEN 5
                    ELSE 1
                END;
            ");

            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Books",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Books",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Authors",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_AuthorId",
                table: "Loans",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Authors_AuthorId",
                table: "Loans",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Authors_AuthorId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_AuthorId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Changes",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Authors",
                newName: "BirthDate");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Books"" 
                ALTER COLUMN ""Genre"" TYPE character varying(100) 
                USING CASE 
                    WHEN ""Genre"" = 1 THEN 'Novel'
                    WHEN ""Genre"" = 2 THEN 'ShortStory'
                    WHEN ""Genre"" = 3 THEN 'Essay'
                    WHEN ""Genre"" = 4 THEN 'Poetry'
                    WHEN ""Genre"" = 5 THEN 'ResearchPaper'
                    ELSE 'Novel'
                END;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Authors",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Authors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
