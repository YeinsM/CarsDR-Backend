using CarSpot.Application.Interfaces;
using CarSpot.Domain;
using CarSpot.Infrastructure.Persistence.Repositories;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CarSpot.Domain.Entities;
using CarSpot.Domain.ValueObjects;



namespace CarSpot.Infrastructure.Persistence.Repositories;
public class UserRepository : IRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsEmailRegisteredAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User> ValidateCredentialsAsync(string email, HashedPassword password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        return null;

    
        if (!user.Password.Matches(password.Value))
        return null;

        return user;
    }


    public async Task<User> RegisterUserAsync(string firstName, string lastName, string email, HashedPassword password, string username)
    {
        
        var user = new User(firstName, lastName, email, password, username);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(int id, string firstName, string lastName, string username)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id {id} not found");

        user.UpdateBasicInfo(firstName, lastName, username);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    Task IRepository<User>.AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    Task IRepository<User>.UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    Task IRepository<User>.DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }
}