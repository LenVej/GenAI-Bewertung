using GenAI_Bewertung.DTOs;
using GenAI_Bewertung.Repositories;

namespace GenAI_Bewertung.Services;

public class StatsService
{
    private readonly IStatsRepository _repository;

    public StatsService(IStatsRepository repository)
    {
        _repository = repository;
    }

    public Task<ProfileStatsDto> GetUserStatsAsync(int userId) => _repository.GetStatsForUserAsync(userId);
}