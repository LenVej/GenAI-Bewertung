﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenAIBewertung.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Questions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Questions");
        }
    }
}
