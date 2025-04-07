using CarSpot.Domain;
using CarSpot.Domain.Entities;
using CarSpot.Domain.ValueObjects;



namespace CarSpot.Application.Interfaces;
    public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> IsEmailRegisteredAsync(string email);
    Task<User> ValidateCredentialsAsync(string email, HashedPassword password);
    Task<User> RegisterUserAsync(string firstName, string lastName, string email, HashedPassword password, string username);
    Task<User> UpdateUserAsync(int id, string firstName, string lastName, string username);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}