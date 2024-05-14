namespace CityBuilder;

static class ArrayExtensions
{
    public static TResult[,] Select<TSource, TResult>(this TSource[,] items, Func<TSource, TResult> function)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        TResult[,] result = new TResult[length0, length1];
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                result[i, j] = function(items[i, j]);
        return result;
    }
    public static (TSource, int, int)[,] Enumerate<TSource>(this TSource[,] items)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        (TSource, int, int)[,] result = new (TSource, int, int)[length0, length1];
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                result[i, j] = (items[i, j], i, j);
        return result;
    }
    public static (TSource, int)[] Enumerate<TSource>(this IEnumerable<TSource> items)
    {
        int length0 = items.Count();
        (TSource, int)[] result = new (TSource, int)[length0];
        for (int i = 0; i < length0; i += 1)
            result[i] = (items.ElementAt(i), i);
        return result;
    }
    public static IEnumerable<TResult> SelectMany<TSource, TResult>(
        this TSource[,] source,
        Func<TSource, IEnumerable<TResult>> selector)
    {
        foreach (TSource item in source)
            foreach (TResult selected in selector(item))
                yield return selected;
    }
    public static IEnumerable<TResult> SelectMany<TSource, TResult>(
        this TSource[,] source,
        Func<TSource, int, IEnumerable<TResult>> selector)
    {
        int i = 0;
        foreach (TSource item in source)
        {
            i++;
            foreach (TResult selected in selector(item, i))
                yield return selected;
        }
    }
    public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
        this TSource[,] source,
        Func<TSource, IEnumerable<TCollection>> collectionSelector,
        Func<TSource, TCollection, TResult> resultSelector)
    {
        foreach (TSource item in source)
        {
            foreach (TCollection collection in collectionSelector(item))
            {
                yield return resultSelector(item, collection);
            }
        }
    }

    public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(
        this TSource[,] source,
        Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
        Func<TSource, TCollection, TResult> resultSelector)
    {
        int i = 0;
        foreach (TSource item in source)
        {
            i++;
            foreach (TCollection collection in collectionSelector(item, i))
                yield return resultSelector(item, collection);
        }
    }
    public static bool Contains<TSource>(this TSource[,] items, TSource element)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                if (element != null && element.Equals(items[i, j]))
                    return true;
        return false;
    }
    public static (int, int)[,] ItemIndexs<TSource>(this TSource[,] items)
    {
        int length0 = items.GetLength(0);
        int length1 = items.GetLength(1);
        (int, int)[,] result = new (int, int)[length0, length1];
        for (int i = 0; i < length0; i += 1)
            for (int j = 0; j < length1; j += 1)
                result[i, j] = (i, j);
        return result;
    }
    public static List<TSource> ToList<TSource>(this TSource[,] items)
    {
        List<TSource> result = [];
        foreach (var item in items)
        {
            result.Add(item);
        }
        return result;
    }
}