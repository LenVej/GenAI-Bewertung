using GenAI_Bewertung.Data;
using GenAI_Bewertung.Entities;
using Microsoft.EntityFrameworkCore;

namespace GenAI_Bewertung.Repositories;

public class ExamRepository : IExamRepository
{
    private readonly ApplicationDbContext _context;

    public ExamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Exam>> GetAllAsync()
    {
        return await _context.Exams
            .Include(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .ToListAsync();
    }

    public async Task<Exam?> GetByIdAsync(int id)
    {
        return await _context.Exams
            .Include(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(e => e.ExamId == id);
    }

    public async Task AddAsync(Exam exam)
    {
        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Exam exam)
    {
        _context.Exams.Remove(exam);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Exam>> GetByUserIdAsync(int userId)
    {
        return await _context.Exams
            .Where(e => e.CreatedBy == userId)
            .Include(e => e.Questions)
            .ThenInclude(eq => eq.Question)
            .ToListAsync();
    }
    
    public async Task UpdateAsync(Exam exam)
    {
        _context.Exams.Update(exam);
        await _context.SaveChangesAsync();
    }

}
