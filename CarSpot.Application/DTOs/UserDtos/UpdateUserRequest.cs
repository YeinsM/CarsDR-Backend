public record UpdateUserRequest(

    string FirstName,
    string LastName,
    string Username,

    string? Phone,
    string? Extension,
    string? CellPhone,
    string? Address,

    Guid BusinessId
    
);
