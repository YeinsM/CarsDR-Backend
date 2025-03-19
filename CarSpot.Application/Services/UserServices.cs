using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Entities;

namespace CarSpot.Application.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetByIdAsync  (int id)
    {
        var user = await _userRepository.GetByIdAsync (id);
        if (user == null || !user.IsActive)
            throw new KeyNotFoundException ("User no fount or inactive.");
            return new UserDto(user.Id, user.Email, user.IsActive  ) ;
    }

    public async Task<int> CreateUserAsync(string email, string password)
    {
        var passwordHash = ByCrypt.Net.BCrypt.HashPassword(password);

        var user = new User ( Email, PasswordHash);
        await _userRepository.AddAsync(user);
        return user.Id;

    }

    public async Task DeactivateUserAsync (int id)
    {
        var user = await _userRepository.GetByIdAsync (id);
        if(user is null)
        throw new KeyNotFoundException ("User not found.");

        user.Deactivate();
        await _userRepository.UpdateAsync(user);
    }

    public async Task ActivateUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if(user is null)
        throw new KeyNotFoundException("User not found.");

        user.Activate();
        await _userRepository.UpdateAsync(user);
    }


}

