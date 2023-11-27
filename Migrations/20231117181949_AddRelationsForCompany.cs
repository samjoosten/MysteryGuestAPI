using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MysteryGuestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsForCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "QuestionForms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "QuestionForms",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "QuestionForms",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "QuestionForms");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "QuestionForms");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "QuestionForms");
        }
    }
}
