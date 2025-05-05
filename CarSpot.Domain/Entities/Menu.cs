namespace CarSpot.Domain.Entities
{
    public class Menu
    {
        public Guid Id { get; private set; }

        public Guid? ParentId { get; private set; }

        public string Label { get; private set; }
        public string Icon { get; private set; }
        public string? To { get; private set; }
        public List<Menu> Children { get; private set; } = new();

        
        public Menu(string label, string icon, string? to, Guid? parentId = null)
        {
            Label = label;
            Icon = icon;
            To = to;
            ParentId = parentId;
        }

        
        public void Update(string label, string icon, string? to, Guid? parentId = null)
        {
            Label = label;
            Icon = icon;
            To = to;
            ParentId = parentId;
        }
    }
}
