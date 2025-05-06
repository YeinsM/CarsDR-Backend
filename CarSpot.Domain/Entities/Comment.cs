public class Comment
{
public Guid Id { get; set; }
public Guid VehicleId { get; set; }
public Guid UserId { get; set; }
public string Content { get; set; }
public DateTime CreatedAt { get; set; }
public Vehicle Vehicle { get; set; }
public User User { get; set; }
}