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

        public DbSet<BlankGap> BlankGaps { get; set; }
        public DbSet<ExamAnswer> ExamAnswers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }

        public DbSet<AiEvaluationResult> AiEvaluationResults { get; set; }
        public DbSet<ExamAttemptEvaluation> ExamAttemptEvaluations { get; set; }

        public DbSet<ExamAttempt> ExamAttempts { get; set; }

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

            // Beziehungen
            modelBuilder.Entity<FillInTheBlankQuestion>()
                .HasMany(q => q.Gaps)
                .WithOne(g => g.FillInTheBlankQuestion)
                .HasForeignKey(g => g.FillInTheBlankQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BlankGap>()
                .ToTable("BlankGaps")
                .Property(g => g.Solutions)
                .HasColumnType("jsonb");

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(eq => eq.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany()
                .HasForeignKey(eq => eq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExamAttempt>()
                .HasMany(ea => ea.Answers)
                .WithOne(a => a.ExamAttempt)
                .HasForeignKey(a => a.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamAnswer>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExamAnswer>()
                .HasOne(a => a.Evaluation)
                .WithOne(ev => ev.ExamAnswer)
                .HasForeignKey<AiEvaluationResult>(ev => ev.ExamAnswerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamAnswer>()
                .Property(a => a.SelectedIndices)
                .HasColumnType("jsonb");

            modelBuilder.Entity<ExamAttempt>()
                .HasOne(a => a.Evaluation)
                .WithOne(e => e.ExamAttempt)
                .HasForeignKey<ExamAttemptEvaluation>(e => e.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔍 Indizes für Performance bei Statistiken
            modelBuilder.Entity<ExamAttempt>()
                .HasIndex(ea => ea.UserId); // für Stats-Abfragen

            modelBuilder.Entity<Question>()
                .HasIndex(q => q.Subject); // für Gruppierung nach Thema
        }
    }
}