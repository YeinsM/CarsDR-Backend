using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;
using CarSpot.Domain.ValueObjects;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CarSpot.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public UserRepository(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }


    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Role)
            .Include(u => u.Vehicles)
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsEmailRegisteredAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsUserRegisteredAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<User?> ValidateCredentialsAsync(string email, HashedPassword password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null || !user.Password.Verify(password.Value))
            return null;

        return user;
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        //var bodyMessage = _emailService.Body(user);
        //var emailSettings = await _context.EmailSettings.FirstOrDefaultAsync(e => e.NickName == "Notifications");

        //if (emailSettings is not null)
        //{
            //await _emailService.SendEmailAsync(user.Email, "Bienvenido al sistema", bodyMessage, emailSettings.NickName!);
       // }

        return user;
    }

    public async Task<User> UpdateUserAsync(Guid id, string firstName, string lastName, string username)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with id {id} not found");

        user.UpdateBasicInfo(firstName, lastName, username);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found.");

        
        user.UpdatedAt = DateTime.UtcNow;
        

        await _context.SaveChangesAsync();

        return user;
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username)
{
    return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
}

}
