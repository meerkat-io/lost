namespace Dust.Utils;

using System.Collections.Generic;

internal class ComponentStorage<T> where T : struct
{
    private int _capacity = 8;
    private int _cursor = 0;
    private T[] _storage;
    private readonly Stack<int> _recycled = new();

    internal ComponentStorage()
    {
        _storage = new T[_capacity];
        Array.Fill(_storage, default);
    }

    internal int Create()
    {
        if (_recycled.Count > 0)
        {
            return _recycled.Pop();
        }

        if (_cursor == _capacity)
        {
            _capacity *= 2;
            Array.Resize(ref _storage, _capacity);
        }

        return _cursor++;
    }

    internal void Recycle(int index)
    {
        _storage[index] = default;
        _recycled.Push(index);
    }

    internal ref T this[int index]
    {
        get => ref _storage[index];
    }

    internal int Capacity => _capacity;
}
