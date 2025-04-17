using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenAIBewertung.Migrations
{
    /// <inheritdoc />
    public partial class QuestionTypeOverhaul : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE ""Questions"" ALTER COLUMN ""QuestionType"" DROP DEFAULT;");
            
            migrationBuilder.Sql(@"
                ALTER TABLE ""Questions""
                ALTER COLUMN ""QuestionType"" TYPE integer
                USING CASE ""QuestionType""
                    WHEN 'MultipleChoice' THEN 0
                    WHEN 'EitherOr' THEN 1
                    WHEN 'OneWord' THEN 2
                    WHEN 'Math' THEN 3
                    WHEN 'Estimation' THEN 4
                    WHEN 'FillInTheBlank' THEN 5
                    WHEN 'FreeText' THEN 6
                    ELSE 0
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QuestionType",
                table: "Questions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}