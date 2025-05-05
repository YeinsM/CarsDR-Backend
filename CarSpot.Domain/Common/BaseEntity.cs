namespace CarSpot.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; set; }
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



        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
