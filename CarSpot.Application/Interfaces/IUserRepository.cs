using CarSpot.Domain;

namespace CarSpot.Application.Interfaces;
    public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> IsEmailRegisteredAsync(string email);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password, string username);
    Task<User> UpdateUserAsync(int id, string firstName, string lastName, string username);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}