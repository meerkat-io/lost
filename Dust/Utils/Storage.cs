namespace Dust.Utils;

using System.Collections.Generic;

public class Storage<T> where T : struct
{
    private int _capacity = 8;
    private int _cursor = 0;
    private T[] _storage;
    private readonly Stack<int> _recycled = new();

    public Storage()
    {
        _storage = new T[_capacity];
        Array.Fill(_storage, default);
    }

    public int Create()
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

    public void Recycle(int index)
    {
        _storage[index] = default;
        _recycled.Push(index);
    }

    public ref T this[int index]
    {
        get => ref _storage[index];
    }

    public int Capacity => _capacity;
}
