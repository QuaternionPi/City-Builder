using System.Diagnostics;

namespace CityBuilder.Map.Generation;

public readonly struct Automata
{
    public enum Cell { Dead = 0, Alive = 1 }
    public readonly Func<int, bool> Born;
    public readonly Func<int, bool> Survive;
    public Automata(Func<int, bool> born, Func<int, bool> survive)
    {
        Born = born;
        Survive = survive;
    }
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
    public static Automata RemoveLonely(int alive)
    {
        Debug.Assert(0 <= alive || 8 >= alive, "alive must be between 0 and 8 inclusive");
        static bool born(int x) => false;
        bool survive(int x) => x > alive;
        return new Automata(born, survive);
    }
    public static Automata FillSurounded(int alive)
    {
        Debug.Assert(0 <= alive || 8 >= alive, "alive must be between 0 and 8 inclusive");
        bool born(int x) => x >= alive;
        static bool survive(int x) => true;
        return new Automata(born, survive);
    }
    public static Automata Random(double density, int seed)
    {
        Debug.Assert(0 < density || 1 > density, "density must be between 0 and 1 inclusive");
        Random random = new(seed);
        bool born(int x) => random.NextDouble() <= density;
        static bool survive(int x) => true;
        return new Automata(born, survive);
    }
    public Cell Evolve(Cell cell, int liveAdjacent)
        => (cell == Cell.Alive
            ? Survive(liveAdjacent) : Born(liveAdjacent))
            ? Cell.Alive : Cell.Dead;
}

public class Grid
{
    private readonly bool LoopX;
    private readonly bool LoopY;
    private int Width { get => Cells.GetLength(0); }
    private int Height { get => Cells.GetLength(1); }
    private int LimitX(int x) => LoopX ? (x + Width) % Width : int.Clamp(x, 0, Width - 1);
    private int LimitY(int y) => LoopY ? (y + Height) % Height : int.Clamp(y, 0, Height - 1);
    private readonly Automata.Cell[,] Cells;
    public Automata.Cell this[int i, int j]
    {
        get => Cells[LimitX(i), LimitY(j)];
        set => Cells[LimitX(i), LimitY(j)] = value;
    }
    private Grid(int x, int y, bool loopX = false, bool loopY = false)
    {
        Debug.Assert(x >= 3, "x must be at least 3");
        Debug.Assert(y >= 3, "y must be at least 3");
        Cells = new Automata.Cell[x, y];
        LoopX = loopX;
        LoopY = loopY;
    }
    private Grid(Grid grid)
    {
        Cells = grid.Cells.Clone() as Automata.Cell[,] ?? throw new NullReferenceException("Clone Cannot be null");
        LoopX = grid.LoopX;
        LoopY = grid.LoopY;
    }
    public Grid Clone()
    {
        return new Grid(this);
    }
    public static Grid Empty(int x, int y)
    {
        return new Grid(x, y, true, false);
    }
    public T[,] Replace<T>(T alive, T dead)
    {
        return
            from cell in Cells
            select cell == Automata.Cell.Alive ? alive : dead;
    }
    public Grid Run(Automata automata, int iterations = 1)
    {
        Debug.Assert(iterations >= 0, "iterations Cannot be Negative");
        if (iterations == 0) return this;

        Grid output = Clone();
        foreach ((Automata.Cell cell, int i, int j) in Cells.Enumerate())
        {
            (int, int)[] adjacent = [
                (i - 1, j - 1),
                (i, j - 1),
                (i + 1, j - 1),
                (i - 1, j),
                (i + 1, j),
                (i - 1, j + 1),
                (i, j + 1),
                (i + 1, j + 1)
            ];
            int liveAdjacent =
                (from point in adjacent
                 where this[point.Item1, point.Item2] == Automata.Cell.Alive
                 select true).Count();
            output.Cells[i, j] = automata.Evolve(cell, liveAdjacent);
        }
        return output.Run(automata, iterations - 1);
    }
    public Grid Poles(int depth, int seed)
    {
        Debug.Assert(depth >= 1, "Poles must be at least 1 cell thick");
        var random = new Random(seed);
        Grid output = Clone();
        foreach ((Automata.Cell cell, int i, int j) in Cells.Enumerate())
        {
            var k = Height - j - 1;
            if (j > depth && k > depth) { continue; }
            else if (j == 0) { output[i, j] = Automata.Cell.Alive; }
            else if (k == 0) { output[i, j] = Automata.Cell.Alive; }
            else
            {
                output[i, j] =
                    output[i, j] == Automata.Cell.Alive
                || ((output[i, j + 1] == Automata.Cell.Alive
                || output[i, j - 1] == Automata.Cell.Alive)
                && random.NextDouble() > 0.5)
                ? Automata.Cell.Alive : Automata.Cell.Dead;
                output[i, j] =
                    output[i, j] == Automata.Cell.Alive
                || ((output[i, k + 1] == Automata.Cell.Alive
                || output[i, k - 1] == Automata.Cell.Alive)
                && random.NextDouble() > 0.5)
                ? Automata.Cell.Alive : Automata.Cell.Dead;
            }
        }
        return output;
    }
    public Grid Border(int border)
    {
        Debug.Assert(border >= 0, "Border cannot be less than 0");
        if (LoopX && LoopY) return BorderXY(border);
        if (LoopX) return BorderY(border);
        if (LoopY) return BorderX(border);
        return this;
    }
    private Grid BorderX(int border)
    {
        Grid output = Clone();
        foreach ((Automata.Cell cell, int i, int j) in Cells.Enumerate())
        {
            output[i, j] = i < border || i - Width > -border ? Automata.Cell.Dead : Cells[i, j];
        }
        return output;
    }
    private Grid BorderY(int border)
    {
        Grid output = Clone();
        foreach ((Automata.Cell cell, int i, int j) in Cells.Enumerate())
        {
            output[i, j] = j < border || j - Height > -border ? Automata.Cell.Dead : Cells[i, j];
        }
        return output;
    }
    private Grid BorderXY(int border)
    {
        return BorderX(border).BorderY(border);
    }
}