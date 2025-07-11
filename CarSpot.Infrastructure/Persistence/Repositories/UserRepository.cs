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

    public async Task<IEnumerable<User>> GetAllBasicAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.Make)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.Model)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.Color)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.Condition)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.Transmission)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.Drivetrain)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.CylinderOption)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.CabType)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.MarketVersion)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.VehicleVersion)
            .Include(u => u.Vehicles)
                .ThenInclude(v => v.MediaFiles)
            .Include(u => u.Comments)
            .ToListAsync();
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
        await _context.SaveChangesAsync(); // Primero guardar para obtener el ID

        // Disparar el evento después de que el usuario tenga un ID válido
        user.NotifyUserRegistered();
        await _context.SaveChangesAsync(); // Guardar nuevamente para procesar el evento

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
