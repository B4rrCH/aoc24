using Microsoft.Extensions.ObjectPool;

namespace Aoc24.IO;

public static class HashSetPool
{
    public static DefaultObjectPool<HashSet<T>> Create<T>() => new(Policy<T>.Instance);

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, ObjectPool<HashSet<T>> pool)
    {
        var set = pool.Get();
        foreach (var item in source)
        {
            set.Add(item);
        }

        return set;
    }

    private sealed class Policy<T> : IPooledObjectPolicy<HashSet<T>>
    {
        public static Policy<T> Instance { get; } = new();

        public HashSet<T> Create() => [];

        public bool Return(HashSet<T> hashSet)
        {
            hashSet.Clear();
            return true;
        }
    }
}