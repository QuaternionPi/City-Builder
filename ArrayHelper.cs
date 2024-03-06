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