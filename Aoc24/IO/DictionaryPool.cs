using Microsoft.Extensions.ObjectPool;

namespace Aoc24.IO;

public static class DictionaryPool
{
    public static DefaultObjectPool<Dictionary<TKey, TValue>> Create<TKey, TValue>()
        where TKey : notnull => new(Policy<TKey, TValue>.Instance);

    private sealed class Policy<TKey, TValue> : IPooledObjectPolicy<Dictionary<TKey, TValue>>
        where TKey : notnull
    {
        public static Policy<TKey, TValue> Instance { get; } = new();

        public Dictionary<TKey, TValue> Create() => [];

        public bool Return(Dictionary<TKey, TValue> dictionary)
        {
            dictionary.Clear();
            return true;
        }
    }
}