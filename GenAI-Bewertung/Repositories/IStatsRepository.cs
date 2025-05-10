using GenAI_Bewertung.DTOs;

namespace GenAI_Bewertung.Repositories;

public interface IStatsRepository
{
    Task<ProfileStatsDto> GetStatsForUserAsync(int userId);
}