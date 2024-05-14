using CityBuilder.Numerics;
using CityBuilder.Map.Structures;
using System.Net;

namespace CityBuilder.Map.Generation;

public static class Generator
{
    private enum Terrain
    {
        Unassigned = 0,
        Beach = 1,
        Grass = 2,
        Forest = 3,
        LowDensity = 4,
        HighDensity = 5,
        Lake = 6,
        Mountain = 7,
        Ocean = 8,
        River = 9,
        Icecap = 10
    }
    private delegate Terrain Evolve(Terrain cell, Terrain[] adjacent);
    private class Feature
    {
        public Feature(Draft parent, int id)
        {
            Parent = parent;
            Id = id;
            _blocks = [];
        }
        private readonly Draft Parent;
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
        public List<Feature> AdjacentFeatures()
        {
            HashSet<Feature> features = [];
            HashSet<(int, int)> toSearch = [];
            int length0 = Parent.Blocks.GetLength(0);
            int length1 = Parent.Blocks.GetLength(1);
            foreach (Block block in Blocks)
            {
                var row = block.Row;
                var col = block.Col;
                (int, int)[] beside = [
                    (row + 1, col),
                    (row - 1, col),
                    (row, col + 1),
                    (row, col - 1)
                ];
                foreach (var spot in beside)
                {
                    var (y, x) = spot;
                    if (0 <= y && length0 > y && 0 <= x && length1 > x)
                    {
                        toSearch.Add(spot);
                    }
                }
            }
            foreach (var (row, col) in toSearch)
            {
                var block = Parent.Blocks[row, col];
                features.Add(block.Feature);
            }
            features.Remove(this);
            return features.ToList();
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
                var feature = new Feature(this, i);
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
            HashSet<(int, int)> searched = [];
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
                    foreach (var spot in beside)
                    {
                        var (y, x) = spot;
                        if ((toSearch.Contains(spot) == false) && (searched.Contains(spot) == false)
                           && 0 <= y && length0 > y && 0 <= x && length1 > x
                           && (input[row, col] == input[y, x]))
                        {
                            toSearch.Push(spot);
                        }
                    }
                }
                partition.Add(part);
            }
            return partition;
        }
    }
    public static Map FromSeed(int x, int y, int seed)
    {
        Grid empty = Grid.Empty(x, y);

        Grid continentsGrid = empty
            .Run(Automata.Random(0.4, seed - 50))
            .Border(4)
            .Run(Automata.Holstein, 10)
            .Run(Automata.Coral, 5)
            .Run(Automata.Bugs, 20)
            .Run(Automata.Coral, 5)
            .Run(Automata.FillSurounded(6), 2);

        Terrain[,] continents = continentsGrid.Replace(Terrain.Grass, Terrain.Ocean);

        Terrain[,] lakes = empty
            .Run(Automata.Random(0.15, seed))
            .Border(8)
            .Run(Automata.Coral, 10)
            .Run(Automata.Bugs, 3)
            .Run(Automata.Smooth, 1)
            .Replace(Terrain.Lake, Terrain.Grass);

        Terrain[,] mountains = empty
            .Run(Automata.Random(0.3, seed + 50))
            .Run(Automata.Coral, 2)
            .Run(Automata.Vote, 15)
            .Run(Automata.Holstein, 5)
            .Run(Automata.RemoveLonely(2))
            .Run(Automata.FillSurounded(6))
            .Replace(Terrain.Mountain, Terrain.Grass);

        Terrain[,] forest = empty
            .Run(Automata.Random(0.43, seed + 100))
            .Run(Automata.Bugs, 12)
            .Run(Automata.RemoveLonely(3), 3)
            .Run(Automata.FillSurounded(7))
            .Replace(Terrain.Forest, Terrain.Grass);

        Terrain[,] cities = empty
            .Run(Automata.Random(0.22, seed + 150))
            .Border(5)
            .Run(Automata.Bugs, 5)
            .Run(Automata.Coral, 10)
            .Run(Automata.Holstein, 5)
            .Run(Automata.Bugs, 2)
            .Run(Automata.Smooth, 4)
            .Replace(Terrain.LowDensity, Terrain.Grass);

        var cullOcean = (Feature f) =>
                f.Terrain == Terrain.Lake
            && f.AdjacentFeatures()
                .Select((Feature f1) => f1.Terrain)
                .Contains(Terrain.Ocean);

        Terrain[,] cells = forest
            .Cover(cities, [Terrain.LowDensity])
            .Cover(lakes, [Terrain.Lake])
            .Cover(mountains, [Terrain.Mountain])
            .Cover(continents, [Terrain.Ocean])
            .ToDraft().Cull(cullOcean, Terrain.Ocean);

        return new Map(ToCells(cells), []);
    }
    private static Terrain[,] Apply(this Terrain[,] input, Evolve evolve, int repeat = 1)
    {
        if (repeat <= 0) return input; // recursion base-case

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
        return Apply(output, evolve, repeat - 1);
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
    private static Terrain[] CellsSmooth(Terrain[,] input)
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
            return [center, center, center, center];
        }

        bool threeSameTop = top == left && top == right;
        bool threeSameRight = right == top && right == bottom;
        bool threeSameBottom = bottom == left && bottom == right;
        bool threeSameLeft = left == top && left == bottom;

        if (threeSameTop && top > center)
        {
            return [top, top, center, top];
        }
        if (threeSameRight && right > center)
        {
            return [right, right, right, center];
        }
        if (threeSameBottom && bottom > center)
        {
            return [center, bottom, bottom, bottom];
        }
        if (threeSameLeft && left > center)
        {
            return [left, center, left, left];
        }

        bool cornerTopLeft = top == left;
        bool cornerTopRight = top == right;
        bool cornerBottomLeft = bottom == left;
        bool cornerBottomRight = bottom == right;

        if (cornerTopLeft && cornerBottomRight && (top > center || bottom > center))
        {
            Terrain topLeft = top > center ? top : center;
            Terrain bottomRight = bottom > center ? bottom : center;
            return [topLeft, bottomRight, bottomRight, topLeft];
        }
        if (cornerTopRight && cornerBottomLeft && (top > center || bottom > center))
        {
            Terrain topRight = top > center ? top : center;
            Terrain bottomLeft = bottom > center ? bottom : center;
            return [topRight, topRight, bottomLeft, bottomLeft];
        }
        if (cornerTopLeft && top > center)
        {
            Terrain topLeft = top;
            return [topLeft, center, center, topLeft];
        }
        if (cornerTopRight && top > center)
        {
            Terrain topRight = top;
            return [topRight, topRight, center, center];
        }
        if (cornerBottomLeft && bottom > center)
        {
            Terrain bottomLeft = bottom;
            return [center, center, bottomLeft, bottomLeft];
        }
        if (cornerBottomRight && bottom > center)
        {
            Terrain bottomRight = bottom;
            return [center, bottomRight, bottomRight, center];
        }

        return [center, center, center, center];
    }
    private static Cell[,] ToCells(this Terrain[,] input)
    {
        int x = input.GetLength(0);
        int y = input.GetLength(1);

        Zone city = new Zone(Color.Orange, "City");

        Cell[,] output = new Cell[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                Terrain[,] neighbourhood = Neighbourhood(input, col, row);
                Terrain[] terrains = CellsSmooth(neighbourhood);
                Land[] lands = terrains.Select((t) =>
                    t.ToLand()).ToArray();
                Zone?[] zones = terrains.Select((t) =>
                    t == Terrain.LowDensity ? city : null).ToArray();
                List<IEnumerable<IStructure>> structures = terrains.Select((t) =>
                    (IEnumerable<IStructure>)(
                        t == Terrain.Forest
                        ? [new Tree()]
                        : []
                    )
                ).ToList();

                output[col, row] = new Cell(col, row, lands, zones, structures);
            }
        }
        return output;
    }
    private static Draft ToDraft(this Terrain[,] terrain) => new(terrain);
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
    private static Land ToLand(this Terrain terrain) => terrain switch
    {
        Terrain.Ocean => Land.Ocean,
        Terrain.River => Land.River,
        Terrain.Lake => Land.Lake,
        Terrain.Beach => Land.Beach,
        Terrain.Grass => Land.Grass,
        Terrain.Forest => Land.Forest,
        Terrain.Mountain => Land.Mountain,
        Terrain.LowDensity => Land.LowDensity,
        Terrain.HighDensity => Land.HighDensity,
        Terrain.Icecap => Land.Icecap,
        _ => throw new Exception($"Cannot conver {terrain} to land"),
    };
}