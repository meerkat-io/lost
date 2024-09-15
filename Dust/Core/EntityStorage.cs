namespace Dust.Core;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class EntityStorage
{
    private readonly int _componentCount = 0;
    private int _capacity = 8;
    private int _cursor = 0;
    private EntityMask[] _masks;
    private Memory<int> _components;
    private readonly Stack<int> _recycled = new();

    internal EntityStorage(int componentCount)
    {
        _componentCount = componentCount;
        _components = new Memory<int>(new int[_capacity * _componentCount]);
        _masks = new EntityMask[_capacity];
    }

    internal Entity Create()
    {
        int index;
        if (_recycled.Count > 0)
        {
            index = _recycled.Pop();
        }
        else
        {
            if (_cursor == _capacity)
            {
                _capacity *= 2;
                var newComponents = new Memory<int>(new int[_capacity * _componentCount]);
                _components.CopyTo(newComponents);
                _components = newComponents;

                Array.Resize(ref _masks, _capacity);
            }

            index = _cursor++;
        }

        Span<int> rowSpan = _components.Span.Slice(index * _componentCount, _componentCount);
        rowSpan.Fill(-1);

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
    internal ref EntityMask GetMask(Entity entity)
    {
        return ref _masks[entity.Id];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsActive(Entity entity)
    {
        return _masks[entity.Id].IsActive();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void AddComponent(Entity entity, ComponentId componentId, int componentIndex)
    {
        _components.Span[entity.Id * _componentCount + componentId.Index] = componentIndex;
        _masks[entity.Id].Set(componentId.Index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool HasComponent(Entity entity, ComponentId componentId)
    {
        return _masks[entity.Id].IsSet(componentId.Index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void RemoveComponent(Entity entity, ComponentId componentId)
    {
        _components.Span[entity.Id * _componentCount + componentId.Index] = -1;
        _masks[entity.Id].Unset(componentId.Index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal int GetComponentIndex(Entity entity, ComponentId componentId)
    {
        return _components.Span[entity.Id * _componentCount + componentId.Index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ForEach(Action<Entity> action)
    {
        for (var i = 0; i < _cursor; i++)
        {
            if (_masks[i].IsActive())
            {
                action(new Entity(i));
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void ForEach(Query query, Action<Entity> action)
    {
        var mask = query._mask;
        for (var i = 0; i < _cursor; i++)
        {
            if (_masks[i].Contains(mask))
            {
                action(new Entity(i));
            }
        }
    }

    internal int CountEntities()
    {
        var count = 0;
        for (var i = 0; i < _cursor; i++)
        {
            if (_masks[i].IsActive())
            {
                count++;
            }
        }
        return count;
    }

    internal int Capacity => _capacity;
}