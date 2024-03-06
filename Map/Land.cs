namespace CityBuilder.Map;

public struct Land
{
    public Color Color { get; set; }
    public bool Buildable { get; set; }
    public Land(Color color, bool buildable)
    {
        Color = color;
        Buildable = buildable;
    }
}