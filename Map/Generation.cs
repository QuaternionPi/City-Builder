using CityBuilder.Geometry;

namespace CityBuilder.Map;

public static class MapGen
{
    private enum Terrain { Unassigned = 0, Beach = 1, Grass = 2, Forest = 3, LowDensity = 4, HighDensity = 5, Lake = 6, Mountain = 7, Ocean = 8, River = 9 }
    private delegate Terrain Evolve(Terrain cell, Terrain[] adjacent);
    private class Feature
    {
        public Feature(int id)
        {
            Id = id;
            _blocks = [];
        }
        private readonly List<Block> _blocks;
        public Terrain Terrain { get { return Blocks.First().Terrain; } }
        public IEnumerable<Block> Blocks { get { return _blocks; } }
        public readonly int Id;
        public int Size() => _blocks.Count();
        public Block CreateBlock(Terrain terrain, int row, int col)
        {
            Block block = new(terrain, row, col, this);
            _blocks.Add(block);
            return block;
        }
    }
    private class Block
    {
        public Block(Terrain terrain, int row, int col, Feature feature)
        {
            Terrain = terrain;
            Row = row;
            Col = col;
            Feature = feature;
        }
        public readonly int Row;
        public readonly int Col;
        public readonly Terrain Terrain;
        public readonly Feature Feature;
    }
    private class Draft
    {
        public Draft(Terrain[,] input)
        {
            Blocks = new Block[input.GetLength(0), input.GetLength(1)];
            var partition = Partition(input);
            int length = partition.Count;
            Features = [];
            for (int i = 0; i < length; i++)
            {
                var part = partition[i];
                var feature = new Feature(i);
                Features.Add(feature);
                foreach (var item in part)
                {
                    var terrain = item.Item1;
                    var row = item.Item2;
                    var col = item.Item3;
                    var block = feature.CreateBlock(terrain, row, col);
                    Blocks[row, col] = block;
                }
            }
        }
        public readonly List<Feature> Features;
        public readonly Block[,] Blocks;
        public Terrain[,] Terrain { get { return from Block in Blocks select Block.Terrain; } }
        public Terrain[,] Cull(Func<Feature, bool> condition, Terrain replace)
        {
            int length0 = Blocks.GetLength(0);
            int length1 = Blocks.GetLength(1);
            Terrain[,] result = new Terrain[length0, length1];
            foreach (Block block in Blocks) result[block.Row, block.Col] = block.Terrain;
            foreach (Feature feature in Features)
            {
                bool cull = condition(feature);
                if (cull == false) continue;
                foreach (Block block in feature.Blocks)
                {
                    result[block.Row, block.Col] = replace;
                }
            }
            return result;
        }
        private static List<List<(Terrain, int, int)>> Partition(Terrain[,] input)
        {
            var nodes = new List<(int, int)>(input.ItemIndexs().ToList());
            int length0 = input.GetLength(0);
            int length1 = input.GetLength(1);
            List<(int, int)> searched = [];
            List<List<(Terrain, int, int)>> partition = [];
            while (nodes.Count != 0)
            {
                var node = nodes.First();
                nodes.Remove(node);
                Stack<(int, int)> toSearch = new Stack<(int, int)>([node]);
                List<(Terrain, int, int)> part = [];
                while (toSearch.Count != 0)
                {
                    (int row, int col) = toSearch.Pop();
                    nodes.Remove((row, col));
                    searched.Add((row, col));
                    part.Add((input[row, col], row, col));

                    (int, int)[] beside = [
                        (row + 1, col),
                        (row - 1, col),
                        (row, col + 1),
                        (row, col - 1)
                    ];
                    var addToSearch =
                        from index in beside
                        where (toSearch.Contains(index) == false)
                           && (searched.Contains(index) == false)
                           && 0 <= index.Item1 && length0 > index.Item1
                           && 0 <= index.Item2 && length1 > index.Item2
                           && (input[row, col] == input[index.Item1, index.Item2])
                        select index;
                    foreach (var item in addToSearch) toSearch.Push(item);
                }
                partition.Add(part);
            }
            return partition;
        }
    }
    private class Automata
    {
        public Automata(Func<int, bool> born, Func<int, bool> survive)
        {
            Born = born;
            Survive = survive;
        }
        public Func<int, bool> Born;
        public Func<int, bool> Survive;
        public static Automata Coral { get { return new Automata((int x) => x >= 3, (int x) => x == 3); } }
        public static Automata Smooth { get { return new Automata((int x) => x >= 4, (int x) => x >= 5); } }
        public static Automata Holstein { get { return new Automata((int x) => x >= 6 || x == 4, (int x) => x >= 5 || x == 3); } }
    }
    public static Map FromSeed(int x, int y, int seed)
    {
        Vector2[] river = [new(0, 0), new(0, 1), new(1, 1), new(1, 2), new(2, 2), new(2, 3), new(3, 3), new(3, 4), new(4, 4)];

        Terrain[,] lakes =
            Create(x, y, Terrain.Grass)
            .RandomAssign(Terrain.Lake, 0.1, seed)
            .Border(Terrain.Grass, 8)
            .RunAutomata(Automata.Coral, Terrain.Lake, Terrain.Grass, 20)
            .RunAutomata(Automata.Smooth, Terrain.Lake, Terrain.Grass, 1)
            .Border(Terrain.Grass, 2);

        Terrain[,] mountains =
            Create(x, y, Terrain.Grass)
            .RandomAssign(Terrain.Mountain, 0.45, seed + 50)
            .RunAutomata(Automata.Holstein, Terrain.Mountain, Terrain.Grass, 45)
            .Apply((Terrain cell, Terrain[] adjacent) => RemoveLonely(cell, adjacent, 3));

        Terrain[,] forest =
            Create(x, y, Terrain.Grass)
            .RandomAssign(Terrain.Forest, 0.5, seed + 100)
            .RunAutomata(Automata.Holstein, Terrain.Forest, Terrain.Grass, 20)
            .Apply((Terrain cell, Terrain[] adjacent) => RemoveLonely(cell, adjacent, 3));

        Terrain[,] cells =
            Create(x, y, Terrain.Grass)
            .RandomAssign(Terrain.LowDensity, 0.2, seed)
            .Border(Terrain.Grass, 5)
            .RunAutomata(Automata.Coral, Terrain.LowDensity, Terrain.Grass, 14)
            .RunAutomata(Automata.Smooth, Terrain.LowDensity, Terrain.Grass, 5)

            .Cover(forest, [Terrain.Forest])
            .Cover(lakes, [Terrain.Lake])
            .Cover(mountains, [Terrain.Mountain])

            .Line(Terrain.River, river, 1)
            .ToDraft().Cull(
                (Feature f) => f.Size() < 40
                            && (f.Terrain == Terrain.LowDensity
                            || f.Terrain == Terrain.Mountain), Terrain.Grass)
            .Apply((Terrain cell, Terrain[] adjacent) => RemoveLonely(cell, adjacent, 2));

        List<Road> roads = [new Road([new(10, 10), new(10, 11), new(10, 12), new(10, 13), new(10, 14), new(11, 15), new(12, 16)])];
        return new Map(ToCells(cells), roads);
    }
    private static Terrain[,] RunAutomata(this Terrain[,] input, Automata automata, Terrain target, Terrain infill, int level)
    {
        Terrain evolution(Terrain cell, Terrain[] adjacent)
        {
            IEnumerable<(Terrain, int)> counts =
                from Terrain in adjacent
                group Terrain by Terrain into block
                select (block.Key, block.Count());
            int targetCount = counts.GetValue(target);

            Terrain output;
            if (cell == target) output = automata.Born(targetCount) ? target : infill;
            else output = automata.Survive(targetCount) ? target : cell;
            return output;
        }
        return input.Apply(evolution, level);
    }
    private static Terrain RemoveLonely(Terrain cell, Terrain[] adjacent, int minSameAdjacent)
    {
        IEnumerable<(Terrain, int)> counts =
            from Terrain in adjacent
            group Terrain by Terrain into block
            select (block.Key, block.Count());
        int sameAdjacentCount = counts.GetValue(cell);

        Terrain output;
        if (sameAdjacentCount >= minSameAdjacent)
        {
            output = cell;
        }
        else
        {
            output = counts.GetHighestKey();
        }
        return output;
    }
    private static Terrain[,] Line(this Terrain[,] input, Terrain terrain, Vector2[] points, float width)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);

        Terrain[,] output = new Terrain[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                Vector2 position = new(col, row);
                bool replace = false;
                foreach (Vector2 point in points)
                {
                    float delta = Vector2.DistanceSquared(position, point);
                    if (delta < width * width)
                    {
                        replace = true;
                        break;
                    }
                }
                output[col, row] = replace ? terrain : input[col, row];
            }
        }
        return output;
    }
    private static Terrain[,] Cover(this Terrain[,] input, Terrain[,] cover, Terrain[] mask)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);

        Terrain[,] output = new Terrain[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                Terrain coverCell = cover[col, row];
                bool replace = mask.Contains(coverCell);
                output[col, row] = replace ? coverCell : input[col, row];
            }
        }
        return output;
    }
    private static Terrain[,] Apply(this Terrain[,] input, Evolve evolve, int level)
    {
        // recursion base-case
        if (level == 0) { return input; }
        Terrain[,] recurse = Apply(input, evolve, level - 1);
        return Apply(recurse, evolve);
    }
    private static Terrain[,] Apply(this Terrain[,] input, Evolve evolve)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);
        Terrain[,] output = new Terrain[x, y];

        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                Terrain cell = input[col, row];
                Terrain[] adjacent = Adjacent(input, col, row);
                output[col, row] = evolve(cell, adjacent);
            }
        }
        return output;
    }
    private static Terrain[,] RandomAssign(this Terrain[,] input, Terrain replace, double threshhold, int seed)
    {
        Random random = new(seed);
        int x = input.GetLength(0);
        int y = input.GetLength(1);

        Terrain[,] output = new Terrain[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                bool passThreshhold = random.NextDouble() <= threshhold;
                output[col, row] = passThreshhold ? replace : input[col, row];
            }
        }
        return output;
    }
    private static Terrain[,] Border(this Terrain[,] input, Terrain replace, int width)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);

        Terrain[,] output = new Terrain[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                bool onBorder = col < width || col >= (x - width) || row < width || row >= (y - width);
                output[col, row] = onBorder ? replace : input[col, row];
            }
        }
        return output;
    }
    private static Color[] CellColorsBasic(Terrain[,] input)
    {
        Color color = input[1, 1].GetColor();
        return [color, color, color, color];
    }
    private static Color[] CellColorsSmooth(Terrain[,] input)
    {
        if (input.GetLength(0) != 3 || input.GetLength(1) != 3) throw new Exception("Input array must be 3x3");
        Terrain center = input[1, 1];
        Terrain top = input[1, 0];
        Terrain right = input[2, 1];
        Terrain bottom = input[1, 2];
        Terrain left = input[0, 1];

        bool allSame = top == left && top == right && top == bottom;
        if (allSame == true)
        {
            Color primary = center.GetColor();
            return [primary, primary, primary, primary];
        }

        bool threeSameTop = top == left && top == right;
        bool threeSameRight = right == top && right == bottom;
        bool threeSameBottom = bottom == left && bottom == right;
        bool threeSameLeft = left == top && left == bottom;

        if (threeSameTop && top > center)
        {
            Color primary = top.GetColor();
            Color secondary = center.GetColor();
            return [primary, primary, secondary, primary];
        }
        if (threeSameRight && right > center)
        {
            Color primary = right.GetColor();
            Color secondary = center.GetColor();
            return [primary, primary, primary, secondary];
        }
        if (threeSameBottom && bottom > center)
        {
            Color primary = bottom.GetColor();
            Color secondary = center.GetColor();
            return [secondary, primary, primary, primary];
        }
        if (threeSameLeft && left > center)
        {
            Color primary = left.GetColor();
            Color secondary = center.GetColor();
            return [primary, secondary, primary, primary];
        }

        bool cornerTopLeft = top == left;
        bool cornerTopRight = top == right;
        bool cornerBottomLeft = bottom == left;
        bool cornerBottomRight = bottom == right;

        if (cornerTopLeft && cornerBottomRight && (top > center || bottom > center))
        {
            Color topLeft = top > center ? top.GetColor() : center.GetColor();
            Color bottomRight = bottom > center ? bottom.GetColor() : center.GetColor();
            return [topLeft, bottomRight, bottomRight, topLeft];
        }
        if (cornerTopRight && cornerBottomLeft && (top > center || bottom > center))
        {
            Color topRight = top > center ? top.GetColor() : center.GetColor();
            Color bottomLeft = bottom > center ? bottom.GetColor() : center.GetColor();
            return [topRight, topRight, bottomLeft, bottomLeft];
        }

        if (cornerTopLeft && top > center)
        {
            Color topLeft = top.GetColor();
            return [topLeft, center.GetColor(), center.GetColor(), topLeft];
        }
        if (cornerTopRight && top > center)
        {
            Color topRight = top.GetColor();
            return [topRight, topRight, center.GetColor(), center.GetColor()];
        }
        if (cornerBottomLeft && bottom > center)
        {
            Color bottomLeft = bottom.GetColor();
            return [center.GetColor(), center.GetColor(), bottomLeft, bottomLeft];
        }
        if (cornerBottomRight && bottom > center)
        {
            Color bottomRight = bottom.GetColor();
            return [center.GetColor(), bottomRight, bottomRight, center.GetColor()];
        }

        Color color = center.GetColor();
        return [color, color, color, color];
    }
    private static Cell[,] ToCells(this Terrain[,] input)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);

        Cell[,] output = new Cell[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                Terrain[,] neighbourhood = Neighbourhood(input, col, row);
                Color[] colors = CellColorsSmooth(neighbourhood);
                output[col, row] = new Cell(col, row, colors);
            }
        }
        return output;
    }
    private static Draft ToDraft(this Terrain[,] terrain) => new(terrain);
    private static bool Contains<T>(this T[,] input, T value)
    {
        if (value == null) throw new NullReferenceException("value cannot be null");
        foreach (T t in input)
        {
            if (value.Equals(t))
            {
                return true;
            }
        }
        return false;
    }
    private static T[] Adjacent<T>(this T[,] input, int xLoc, int yLoc)
    {
        T[,] cells = Neighbourhood(input, xLoc, yLoc);
        return [cells[0, 0], cells[0, 1], cells[0, 2], cells[1, 0], cells[1, 2], cells[2, 0], cells[2, 1], cells[2, 2]];
    }
    private static T[,] Neighbourhood<T>(this T[,] input, int xLoc, int yLoc)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);
        T[,] output = new T[3, 3];
        for (int col = 0; col < 3; col++)
        {
            for (int row = 0; row < 3; row++)
            {
                int colOffset = Math.Clamp(col - 1 + xLoc, 0, x - 1);
                int rowOffset = Math.Clamp(row - 1 + yLoc, 0, y - 1);
                output[col, row] = input[colOffset, rowOffset];
            }
        }
        return output;
    }
    private static TValue? GetValue<TKey, TValue>(this IEnumerable<(TKey, TValue?)> keyValuePairs, TKey targetKey)
    {
        if (targetKey == null) throw new NullReferenceException("targetKey cannot be null");
        foreach ((TKey key, TValue? value) in keyValuePairs)
        {
            if (targetKey.Equals(key))
            {
                return value;
            }
        }
        return default;
    }
    private static TKey GetHighestKey<TKey, TValue>(this IEnumerable<(TKey, TValue?)> keyValuePairs)
    {
        return
            (from pair in keyValuePairs
             orderby pair.Item2
             select pair.Item1).Last();

    }
    private static Terrain[,] Create(int x, int y, Terrain terrain)
    {
        Terrain[,] output = new Terrain[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                output[col, row] = terrain;
            }
        }
        return output;
    }
    private static Color GetColor(this Terrain Terrain) => Terrain switch
    {
        Terrain.Ocean => Color.DARKBLUE,
        Terrain.River => Color.BLUE,
        Terrain.Lake => new Color(88, 88, 255, 255),
        Terrain.Beach => new Color(255, 216, 146, 255),
        Terrain.Grass => Color.GREEN,
        Terrain.Forest => Color.DARKGREEN,
        Terrain.Mountain => Color.WHITE,
        Terrain.LowDensity => Color.GRAY,
        Terrain.HighDensity => Color.DARKGRAY,
        _ => Color.BLACK,
    };
}