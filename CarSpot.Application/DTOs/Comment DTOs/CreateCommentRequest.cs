public record CreateCommentRequest
(
    Guid VehicleId,
    Guid UserId,
    string Content = null!
);