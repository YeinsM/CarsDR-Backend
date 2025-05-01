namespace CarSpot.Domain.Entities
{
    public class Menu
    {
        public int Id { get; private set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public List<Menu[]> Menub { get; set; } = new();
        public string? To { get; set; }

        public Menu(string label, string icon, List<Menu[]> menub, string? to)
        {
            Label = label;
            Icon = icon;
            Menub = menub;
            To = to;
        }

        public void Update(string label, string icon, List<Menu[]> menub, string? to)
        {
            Label = label;
            Icon = icon;
            Menub = menub;
            To = to;
        }
    }
}
