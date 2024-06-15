using System.Collections.Generic;
using System.Linq;

public static class LinqUtil
{
    public static List<(T, T)> Permutation<T>(List<T> a, List<T> b)
    {
        return (from x in a
                from y in b
                select (x, y)).ToList();
    }
}