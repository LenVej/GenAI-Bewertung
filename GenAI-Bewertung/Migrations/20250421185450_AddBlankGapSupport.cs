using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GenAIBewertung.Migrations
{
    /// <inheritdoc />
    public partial class AddBlankGapSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Solutions",
                table: "FillInTheBlankQuestions");

            migrationBuilder.CreateTable(
                name: "BlankGaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Solutions = table.Column<List<string>>(type: "jsonb", nullable: false),
                    FillInTheBlankQuestionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlankGaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlankGaps_FillInTheBlankQuestions_FillInTheBlankQuestionId",
                        column: x => x.FillInTheBlankQuestionId,
                        principalTable: "FillInTheBlankQuestions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlankGaps_FillInTheBlankQuestionId",
                table: "BlankGaps",
                column: "FillInTheBlankQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlankGaps");

            migrationBuilder.AddColumn<List<string>>(
                name: "Solutions",
                table: "FillInTheBlankQuestions",
                type: "text[]",
                nullable: false);
        }
    }
}
