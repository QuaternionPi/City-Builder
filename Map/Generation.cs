using CityBuilder.Numerics;
using CityBuilder.Map.Structures;

namespace CityBuilder.Map;

public static class MapGen
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
    private class Automata
    {
        public Automata(Func<int, bool> born, Func<int, bool> survive)
        {
            Born = born;
            Survive = survive;
        }
        public Func<int, bool> Born;
        public Func<int, bool> Survive;
        public static Automata Coral
        {
            get { return new Automata((int x) => x == 3, (int x) => x >= 3); }
        }
        public static Automata Smooth
        {
            get { return new Automata((int x) => x >= 5, (int x) => x >= 4); }
        }
        public static Automata Holstein
        {
            get { return new Automata((int x) => x >= 5 || x == 3, (int x) => x >= 6 || x == 4); }
        }
        public static Automata Bugs
        {
            get { return new Automata((int x) => x >= 3 && x != 4 && x != 8, (int x) => x >= 5 || x == 1); }
        }
        public static Automata Vote
        {
            get { return new Automata((int x) => x >= 5, (int x) => x >= 4); }
        }
    }
    public static Map FromSeed(int x, int y, int seed)
    {
        Terrain[,] map_base = Create(x, y, Terrain.Grass);

        Terrain[,] continents = Create(x, y, Terrain.Ocean)
            .RandomAssign(Terrain.Grass, 0.4, seed - 50)
            .Border(Terrain.Ocean, 4)
            .RunAutomataChain(Terrain.Grass, Terrain.Ocean, [
                (Automata.Holstein, 10),
                (Automata.Coral, 5),
                (Automata.Bugs, 20),
                (Automata.Coral, 5)
            ]);

        Terrain[,] lakes = map_base
            .RandomAssign(Terrain.Lake, 0.15, seed)
            .Border(Terrain.Grass, 8)
            .RunAutomataChain(Terrain.Lake, Terrain.Grass, [
                (Automata.Coral, 10),
                (Automata.Bugs, 3),
                (Automata.Smooth, 1)
            ])
            .Border(Terrain.Grass, 2);

        Terrain[,] mountains = map_base
            .RandomAssign(Terrain.Mountain, 0.4, seed + 50)
            .RunAutomataChain(Terrain.Mountain, Terrain.Grass, [
                (Automata.Coral, 1),
                (Automata.Vote, 15),
                (Automata.Holstein, 15)
            ])
            .RandomAssign(Terrain.Grass, 0.3, seed + 50)
            .RunAutomata(Terrain.Mountain, Terrain.Grass, Automata.Holstein, 15)
            .Apply((Terrain cell, Terrain[] adjacent) => RemoveLonely(cell, adjacent, 3));

        Terrain[,] forest = map_base
            .RandomAssign(Terrain.Forest, 0.43, seed + 100)
            .RunAutomata(Terrain.Forest, Terrain.Grass, Automata.Bugs, 20)
            .Apply((Terrain cell, Terrain[] adjacent) => RemoveLonely(cell, adjacent, 3));

        Terrain[,] cities = map_base
            .RandomAssign(Terrain.LowDensity, 0.22, seed + 150)
            .Border(Terrain.Grass, 5)
            .RunAutomataChain(Terrain.LowDensity, Terrain.Grass, [
                (Automata.Bugs, 5),
                (Automata.Coral, 10),
                (Automata.Holstein, 5),
                (Automata.Bugs, 2),
                (Automata.Smooth, 4),
            ]);

        var cullSize = (Feature f) =>
                f.Size() < 50
            && (f.Terrain == Terrain.Mountain
             || f.Terrain == Terrain.Ocean
             || f.Terrain == Terrain.Forest
             || f.Terrain == Terrain.Ocean)
             || (f.Size() > 150 && f.Terrain == Terrain.Mountain);

        var cullOcean = (Feature f) =>
                f.Terrain == Terrain.Lake
            && f.AdjacentFeatures()
                .Select((Feature f1) => f1.Terrain)
                .Contains(Terrain.Ocean);

        Terrain[,] cells =
            forest
            .Cover(cities, [Terrain.LowDensity])
            .Cover(lakes, [Terrain.Lake])
            .Cover(mountains, [Terrain.Mountain])
            .Cover(continents, [Terrain.Ocean])
            .ToDraft().Cull(cullSize, Terrain.Grass)
            .Apply((Terrain cell, Terrain[] adjacent) => RemoveLonely(cell, adjacent, 3))
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
    private static Terrain[,] RunAutomata(this Terrain[,] input, Terrain live, Terrain dead, Automata automata, int repeat)
    {
        Terrain evolution(Terrain cell, Terrain[] adjacent)
        {
            IEnumerable<(Terrain, int)> counts =
                from Terrain in adjacent
                group Terrain by Terrain into block
                select (block.Key, block.Count());
            int liveCount = counts.GetValue(live);

            Terrain output;
            if (cell == live) output = automata.Survive(liveCount) ? live : dead; // if cell is alive does it stay alive
            else output = automata.Born(liveCount) ? live : cell; // if cell is dead does it become alive
            return output;
        }
        return input.Apply(evolution, repeat);
    }
    private static Terrain[,] RunAutomataChain(this Terrain[,] input, Terrain live, Terrain dead, IEnumerable<(Automata, int)> steps)
    {
        foreach ((Automata automata, int repeat) in steps)
            input = RunAutomata(input, live, dead, automata, repeat);
        return input;
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
    private static Terrain[] CellsBasic(Terrain[,] input)
    {
        Terrain terrain = input[1, 1];
        return [terrain, terrain, terrain, terrain];
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
    private static T[,] Create<T>(int x, int y, T value)
    {
        T[,] output = new T[x, y];
        for (int col = 0; col < x; col++)
        {
            for (int row = 0; row < y; row++)
            {
                output[col, row] = value;
            }
        }
        return output;
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