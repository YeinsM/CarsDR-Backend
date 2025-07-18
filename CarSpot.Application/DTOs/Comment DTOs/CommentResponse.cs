public record CommentResponse(
    Guid Id,
    string Content,
    Guid UserId,  
    string AuthorName,
    DateTime CreatedAt,
    bool IsReported,
    List<CommentResponse>? Replies
);
