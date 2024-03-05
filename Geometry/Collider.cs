namespace CityBuilder.Geometry;

public class Collider
{
    public Collider(IPolygon polygon)
    {
        Polygon = polygon;
    }
    public readonly IPolygon Polygon;
    public static bool Collidies(Collider col1, Collider col2)
    {
        Vector2[] poly1 = col1.Polygon.Points;
        Vector2[] poly2 = col2.Polygon.Points;
        return PolygonsOverlap(poly1, poly2);
    }
    public static bool Collidies(Collider col, Vector2 point)
    {
        Vector2[] poly = col.Polygon.Points;
        int len = poly.Length;
        bool inside = false;

        // Store the first point in the polygon and initialize the second point
        Vector2 p1 = poly[0];

        // Loop through each edge in the polygon
        for (int i = 1; i <= len; i++)
        {
            // Get the next point in the polygon
            Vector2 p2 = poly[i % len];

            // Check if the point is above the minimum y coordinate of the edge
            bool within_y = point.Y > Math.Min(p1.Y, p2.Y) && point.Y <= Math.Max(p1.Y, p2.Y);
            // Check if the point is to the left of the maximum x coordinate of the edge
            if (within_y && point.X <= Math.Max(p1.X, p2.X))
            {
                // Calculate the x-intersection of the line connecting the point to the edge
                double xIntersection = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;

                // Check if the point is on the same line as the edge or to the left of the x-intersection
                if (p1.X == p2.X || point.X <= xIntersection)
                {
                    // Flip the inside flag
                    inside = !inside;
                }
            }

            // Store the current point as the first point for the next iteration
            p1 = p2;
        }

        // Return the value of the inside flag
        return inside;
    }
    private static Vector2[] GetAxes(Vector2[] poly)
    {
        int j;
        int len = poly.Length;
        Vector2[] axes = new Vector2[len];
        Vector2 vector1, vector2, edge;
        for (int i = 0; i < len; ++i)
        {
            vector1 = poly[i];
            j = (i + 1 == len) ? 0 : i + 1;
            vector2 = poly[j];
            edge = vector1 - vector2;
            axes[i] = new Vector2(-edge.Y, edge.X);
        }
        return axes;
    }
    private static Projection ProjectOntoAxis(Vector2[] poly, Vector2 axis)
    {
        int len = poly.Length;
        Vector2 vector0, vector;
        float min, max, p;
        vector0 = poly[0];
        min = Vector2.Dot(axis, vector0);
        max = min;
        for (int i = 1; i < len; ++i)
        {
            vector = poly[i];
            p = Vector2.Dot(axis, vector);
            if (p < min)
            {
                min = p;
            }
            else if (p > max)
            {
                max = p;
            }
        }
        return new Projection(min, max);
    }
    private static bool ProjectionsOverlap(Projection proj1, Projection proj2)
    {
        if (proj1.Max < proj2.Min) return false;
        if (proj2.Max < proj1.Min) return false;
        return true;
    }
    private static bool PolygonsOverlap(Vector2[] poly1, Vector2[] poly2)
    {
        Vector2[] axes1 = GetAxes(poly1);
        Vector2[] axes2 = GetAxes(poly2);
        Projection proj1, proj2;

        foreach (var axis in axes1)
        {
            proj1 = ProjectOntoAxis(poly1, axis);
            proj2 = ProjectOntoAxis(poly2, axis);
            if (!ProjectionsOverlap(proj1, proj2)) return false;
        }
        foreach (var axis in axes1)
        {
            proj1 = ProjectOntoAxis(poly1, axis);
            proj2 = ProjectOntoAxis(poly2, axis);
            if (!ProjectionsOverlap(proj1, proj2)) return false;
        }
        return true;
    }
    private readonly struct Projection
    {
        public Projection(float min, float max)
        {
            Min = min;
            Max = max;
        }
        public readonly float Min;
        public readonly float Max;
    }
}

