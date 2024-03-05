namespace CityBuilder;

static class ArrayExtensions
{
    public static R[,] Select<T, R>(this T[,] items, Func<T, R> f)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        R[,] result = new R[length0, length1];
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                result[i, j] = f(items[i, j]);
        return result;
    }
    public static bool Contains<T>(this T[,] items, T element)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                if (element != null && element.Equals(items[i, j]))
                    return true;
        return false;
    }
    public static (int, int)[,] ItemIndexs<T>(this T[,] items)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        (int, int)[,] result = new (int, int)[length0, length1];
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                result[i, j] = (i, j);
        return result;
    }
    public static List<T> ToList<T>(this T[,] items)
    {
        List<T> result = [];
        foreach (var item in items)
        {
            result.Add(item);
        }
        return result;
    }
}