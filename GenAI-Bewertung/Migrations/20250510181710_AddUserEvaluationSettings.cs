using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenAIBewertung.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEvaluationSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CaseSensitive",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EstimateTolerance",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tolerance",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseSensitive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EstimateTolerance",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Tolerance",
                table: "Users");
        }
    }
}
