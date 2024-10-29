using GenAI_Bewertung.Models;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; }  // Beispiel: Frage-Typ
    public DbSet<Answer> Answers { get; set; }      // Beispiel: Antworten
    // Füge hier weitere DbSets für andere Entitäten hinzu
}