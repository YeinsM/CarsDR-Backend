using CarSpot.Domain.Entities;

namespace CarSpot.Application.interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task AddAsync (User user);
}