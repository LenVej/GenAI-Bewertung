using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenAIBewertung.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionSubtypesTPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EitherOrQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    OptionA = table.Column<string>(type: "text", nullable: false),
                    OptionB = table.Column<string>(type: "text", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EitherOrQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_EitherOrQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstimationQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    CorrectValue = table.Column<double>(type: "double precision", nullable: false),
                    TolerancePercent = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimationQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_EstimationQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FillInTheBlankQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    ClozeText = table.Column<string>(type: "text", nullable: false),
                    Solutions = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillInTheBlankQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_FillInTheBlankQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FreeTextQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    ExpectedKeywords = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeTextQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_FreeTextQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MathQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    ExpectedResult = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MathQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_MathQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    Choices = table.Column<List<string>>(type: "text[]", nullable: false),
                    CorrectIndices = table.Column<List<int>>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OneWordQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "integer", nullable: false),
                    ExpectedAnswer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneWordQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_OneWordQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EitherOrQuestions");

            migrationBuilder.DropTable(
                name: "EstimationQuestions");

            migrationBuilder.DropTable(
                name: "FillInTheBlankQuestions");

            migrationBuilder.DropTable(
                name: "FreeTextQuestions");

            migrationBuilder.DropTable(
                name: "MathQuestions");

            migrationBuilder.DropTable(
                name: "MultipleChoiceQuestions");

            migrationBuilder.DropTable(
                name: "OneWordQuestions");
        }
    }
}
