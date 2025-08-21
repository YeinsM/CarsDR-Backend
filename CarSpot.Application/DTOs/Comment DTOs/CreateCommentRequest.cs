public record CreateCommentRequest(
    Guid ListingId,
    Guid UserId,
    string Text,
    Guid? ParentCommentId
);
