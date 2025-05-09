using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GenAIBewertung.Migrations
{
    /// <inheritdoc />
    public partial class AddExamAttemptEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamAttemptEvaluations",
                columns: table => new
                {
                    ExamAttemptEvaluationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamAttemptId = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: false),
                    FeedbackSummary = table.Column<string>(type: "text", nullable: false),
                    EvaluatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAttemptEvaluations", x => x.ExamAttemptEvaluationId);
                    table.ForeignKey(
                        name: "FK_ExamAttemptEvaluations_ExamAttempts_ExamAttemptId",
                        column: x => x.ExamAttemptId,
                        principalTable: "ExamAttempts",
                        principalColumn: "ExamAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamAttemptEvaluations_ExamAttemptId",
                table: "ExamAttemptEvaluations",
                column: "ExamAttemptId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamAttemptEvaluations");
        }
    }
}
