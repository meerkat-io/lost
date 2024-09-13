namespace Dust.Core;

using System.Collections.Generic;

internal class ComponentChunk<T> where T : struct
{
    private int _capacity = 8;
    private int _cursor = 0;
    private T[] _chunk;
    private readonly Stack<int> _recycled = new();

    internal ComponentChunk()
    {
        _chunk = new T[_capacity];
        Array.Fill(_chunk, default);
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
            Array.Resize(ref _chunk, _capacity);
        }

        return _cursor++;
    }

    internal void Recycle(int index)
    {
        _chunk[index] = default;
        _recycled.Push(index);
    }

    internal ref T this[int index]
    {
        get => ref _chunk[index];
    }

    internal int Capacity => _capacity;
}
