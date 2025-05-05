using CarSpot.Domain.Common;

public class Color : BaseEntity
{
    public string Name { get; private set; }

    public Color(string name)
    {
        
        Name = name;
    }
}
