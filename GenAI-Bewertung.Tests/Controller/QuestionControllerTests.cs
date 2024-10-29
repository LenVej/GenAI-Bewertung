using GenAI_Bewertung.Controllers;
using GenAI_Bewertung.Data;
using GenAI_Bewertung.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Tests.Controller
{
    public class QuestionsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly QuestionsController _controller;

        public QuestionsControllerTests()
        {
            // In-Memory-Datenbank einrichten
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);

            // Sample-Daten hinzufügen
            _context.Questions.AddRange(
                new Question {},
                new Question {}
            );
            _context.SaveChanges();

            // Controller initialisieren
            _controller = new QuestionsController(_context);
        }
    }
}
