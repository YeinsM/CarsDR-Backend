using CarSpot.Application.DTOs;

namespace CarSpot.Application.interfaces;

    public interface IUserServices
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<int> CreateUserAsync(string email, string password);
        Task DeactivateUserAsync(int id);
        Task ActivateUserAsync(int id); 
    }
