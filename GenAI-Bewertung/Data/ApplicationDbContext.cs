using GenAI_Bewertung.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; }  
    public DbSet<Answer> Answers { get; set; }      
    public DbSet<User> Users { get; set; }
    // Füge hier weitere DbSets für andere Entitäten hinzu
}