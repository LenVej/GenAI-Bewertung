using GenAI_Bewertung.Entities;
using System.Threading.Tasks;

namespace GenAI_Bewertung.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task DeleteUserAsync(User user);
    }
}