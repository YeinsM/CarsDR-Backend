using CarSpot.Domain.Common;
using CarSpot.Domain.Entities;

public class Comment : BaseEntity
{
    public string? Content { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }= default!;

    public Guid ListingId { get; set; }
    public Listing? Listing { get; set; }
    public bool IsReported { get; set; } = false;

    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}
