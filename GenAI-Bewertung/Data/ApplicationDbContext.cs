using GenAI_Bewertung.Entities;
using GenAI_Bewertung.Entities.QuestionTypes;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets für Haupt- und Subklassen
        public DbSet<Question> Questions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<EitherOrQuestion> EitherOrQuestions { get; set; }
        public DbSet<OneWordQuestion> OneWordQuestions { get; set; }
        public DbSet<MathQuestion> MathQuestions { get; set; }
        public DbSet<EstimationQuestion> EstimationQuestions { get; set; }
        public DbSet<FillInTheBlankQuestion> FillInTheBlankQuestions { get; set; }
        public DbSet<FreeTextQuestion> FreeTextQuestions { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Haupttabelle
            modelBuilder.Entity<Question>().ToTable("Questions");

            // Subtypen als TPT-Mapping
            modelBuilder.Entity<MultipleChoiceQuestion>().ToTable("MultipleChoiceQuestions");
            modelBuilder.Entity<EitherOrQuestion>().ToTable("EitherOrQuestions");
            modelBuilder.Entity<OneWordQuestion>().ToTable("OneWordQuestions");
            modelBuilder.Entity<MathQuestion>().ToTable("MathQuestions");
            modelBuilder.Entity<EstimationQuestion>().ToTable("EstimationQuestions");
            modelBuilder.Entity<FillInTheBlankQuestion>().ToTable("FillInTheBlankQuestions");
            modelBuilder.Entity<FreeTextQuestion>().ToTable("FreeTextQuestions");

            // Optional: Weitere Konfigurationen hier (z. B. Beziehungen, Constraints etc.)
        }
    }
}
