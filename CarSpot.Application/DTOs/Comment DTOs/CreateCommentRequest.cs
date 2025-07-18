public record CreateCommentRequest(
    string? Content,
    Guid ListingId,
    Guid UserId,
    string Text,
    Guid? ParentCommentId
);
