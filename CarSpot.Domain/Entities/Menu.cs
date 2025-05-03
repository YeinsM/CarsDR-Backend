namespace CarSpot.Domain.Entities
{
    public class Menu
    {
        public int Id { get; private set; }

        public int? ParentId { get; private set; }

        public string Label { get; private set; }
        public string Icon { get; private set; }
        public string? To { get; private set; }
        public List<Menu> Children { get; private set; } = new();

        
        public Menu(string label, string icon, string? to, int? parentId = null)
        {
            Label = label;
            Icon = icon;
            To = to;
            ParentId = parentId;
        }

        
        public void Update(string label, string icon, string? to, int? parentId = null)
        {
            Label = label;
            Icon = icon;
            To = to;
            ParentId = parentId;
        }
    }
}
