namespace CarSpot.Domain.Common
{
   public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
   
    protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

    public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
    
    }
}
