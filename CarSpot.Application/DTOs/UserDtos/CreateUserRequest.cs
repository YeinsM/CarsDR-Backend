public record CreateUserRequest(

    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password,
    Guid RoleId,
    string? Phone,
    string? Extension,
    string? CellPhone,
    string? Address,
    Guid BusinessId

);
