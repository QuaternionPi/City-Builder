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
    static public Land Ocean { get { return new Land(Color.DarkBlue, false); } }
    static public Land River { get { return new Land(Color.Blue, false); } }
    static public Land Lake { get { return new Land(new Color(88, 88, 255, 255), false); } }
    static public Land Beach { get { return new Land(new Color(255, 216, 146, 255), true); } }
    static public Land Grass { get { return new Land(Color.Green, true); } }
    static public Land Forest { get { return new Land(Color.DarkGreen, true); } }
    static public Land Mountain { get { return new Land(Color.LightGray, false); } }
    static public Land LowDensity { get { return new Land(Color.Gray, true); } }
    static public Land HighDensity { get { return new Land(Color.DarkGray, true); } }
    static public Land Icecap { get { return new Land(Color.White, false); } }
}