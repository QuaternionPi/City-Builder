namespace CityBuilder.Map;

public class Zone
{
    public Color Color { get; set; }
    public string Name { get; set; }
    public Zone(Color color, string name)
    {
        Color = color;
        Name = name;
    }
}