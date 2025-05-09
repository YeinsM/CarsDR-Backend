using CarSpot.Domain.Entities;
using CarSpot.Domain.ValueObjects;

namespace CarSpot.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(Guid id);
        Task<User> UpdateAsync(Guid id);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<User?> ValidateCredentialsAsync(string email, HashedPassword password);

        Task<User> RegisterUserAsync(User user);

        Task<User> UpdateUserAsync(Guid id, string firstName, string lastName, string username);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
