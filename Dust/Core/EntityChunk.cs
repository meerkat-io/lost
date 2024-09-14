namespace Dust.Core;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class EntityChunk
{
    private readonly int _componentCount = 0;
    private int _capacity = 8;
    private int _cursor = 0;
    private EntityMask[] _masks;
    private int[,] _componentIndexes;
    private readonly Stack<int> _recycled = new();

    internal EntityChunk(int componentCount)
    {
        _componentCount = componentCount;
        _componentIndexes = new int[_capacity, _componentCount];
        _masks = new EntityMask[_capacity];
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

        fixed (int* ptr = _componentIndexes)
        {
            Span<int> rowSpan = new(ptr + index * _componentCount, _componentCount);
            rowSpan.Fill(-1);
        }
        _masks[index].Clear();
        _masks[index].Activate();

        return new Entity(index);
    }

    internal void Recycle(Entity entity)
    {
        _masks[entity.Id].Deactivate();
        _recycled.Push(entity.Id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal unsafe Span<int> GetComponentIndexes(Entity entity)
    {
        fixed (int* ptr = _componentIndexes)
        {
            return new Span<int>(ptr + entity.Id * _componentCount, _componentCount);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref EntityMask GetMask(Entity entity)
    {
        return ref _masks[entity.Id];
    }

    internal int Capacity => _capacity;
}