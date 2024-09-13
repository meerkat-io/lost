namespace Dust.Core;

using System.Collections.Generic;

internal class EntityChunk
{
    private readonly int _componentCount = 0;
    private int _capacity = 8;
    private int _cursor = 0;
    private int[] _masks;
    private int[,] _componentIndexes;
    private readonly Stack<int> _recycled = new();

    internal EntityChunk(int componentCount)
    {
        _componentCount = componentCount;
        _componentIndexes = new int[_capacity, _componentCount];
        _masks = new int[_capacity];
    }

    internal unsafe Entity Create()
    {
        int index = -1;
        if (_recycled.Count > 0)
        {
            index = _recycled.Pop();
        }
        else
        {
            if (_cursor == _capacity)
            {
                int newCapacity = _capacity * 2;
                int[,] newComponentIndexes = new int[newCapacity, _componentCount];

                fixed (int* oldPtr = _componentIndexes, newPtr = newComponentIndexes)
                {
                    Span<int> oldSpan = new(oldPtr, _componentCount * _capacity);
                    Span<int> newSpan = new(newPtr, _componentCount * _capacity);
                    oldSpan.CopyTo(newSpan);
                }
                Array.Resize(ref _masks, newCapacity);

                _componentIndexes = newComponentIndexes;
                _capacity = newCapacity;
            }

            index = _cursor++;
        }
        for (var i = 0; i < _componentCount; i++)
        {
            _componentIndexes[index, i] = -1;
        }

        fixed (int* ptr = _componentIndexes)
        {
            Span<int> rowSpan = new(ptr + index * _componentCount, _componentCount);
            rowSpan.Fill(-1);
        }
        _masks[index] = 1; // Entity is alive (bit 0)
        return new Entity(index);
    }

    internal void Recycle(Entity entity)
    {
        _recycled.Push(entity.Id);
    }

    internal unsafe Span<int> GetComponentIndexes(Entity entity)
    {
        fixed (int* ptr = _componentIndexes)
        {
            return new Span<int>(ptr + entity.Id * _componentCount, _componentCount);
        }
    }

    internal ref int GetMask(Entity entity)
    {
        return ref _masks[entity.Id];
    }

    internal int Capacity => _capacity;
}