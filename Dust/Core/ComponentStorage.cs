namespace Dust.Core;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class ComponentStorage
{
    private int _capacity = 8;
    private int _cursor = 0;
    private int _componentSize = 0;
    private Memory<byte> _storage;
    private readonly Stack<int> _recycled = new();

    internal ComponentStorage(Type componentType)
    {
        _componentSize = Marshal.SizeOf(componentType);
        _storage = new Memory<byte>(new byte[_capacity * _componentSize]);
    }

    internal int Create()
    {
        if (_recycled.Count > 0)
        {
            int index = _recycled.Pop();
            _storage.Span.Slice(index * _componentSize, _componentSize).Clear();
            return index;
        }
        else if (_cursor < _capacity)
        {
            return _cursor++;
        }
        else
        {
            _capacity *= 2;
            var newStorage = new Memory<byte>(new byte[_capacity * _componentSize]);
            _storage.CopyTo(newStorage);
            _storage = newStorage;
            return _cursor++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Recycle(int index)
    {
        _recycled.Push(index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref T GetItem<T>(int index) where T : struct
    {
        return ref Unsafe.As<byte, T>(ref _storage.Span[index * _componentSize]);
    }

    internal int Capacity => _capacity;
}
