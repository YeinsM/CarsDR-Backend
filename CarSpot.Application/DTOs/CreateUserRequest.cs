namespace CarSpot.Application.DTOs;
public record CreateUserRequest(string Username, string FirstName, string LastName, string Email, string Password);
