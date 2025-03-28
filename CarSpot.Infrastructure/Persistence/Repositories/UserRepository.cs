using CarSpot.Application.Interfaces;
using CarSpot.Domain;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories;
public class UserRepository : IUserRepository
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

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public async Task<User> RegisterUserAsync(string firstName, string lastName, string email, string password, string username)
    {
        var user = new User(firstName, lastName, email, password, username);
        await _context.Users.AddAsync(user);
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
}