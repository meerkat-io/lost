namespace Dust.Core;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class ComponentRegistry
{
    private const int _maxComponents = 31;
    private int _cursor = 0;
    private readonly int[] _registry = new int[_maxComponents];
    private readonly int[] _componentSize = new int[_maxComponents];

    internal ComponentId Register<T>() where T : struct
    {
        if (_cursor == _maxComponents)
        {
            throw new IndexOutOfRangeException("Component registry is full");
        }

        var hashCode = typeof(T).GetHashCode();
        for (var i = 0; i < _cursor; i++)
        {
            if (_registry[i] == hashCode)
            {
                throw new DuplicateComponentException("Component type already registered");
            }
        }

        var index = _cursor++;
        _registry[index] = hashCode;
        _componentSize[index] = Unsafe.SizeOf<T>();

        return new ComponentId(index);
    }

    internal ComponentId GetComponentId<T>() where T : struct
    {
        var hashCode = typeof(T).GetHashCode();
        for (var i = 0; i < _cursor; i++)
        {
            if (_registry[i] == hashCode)
            {
                return new ComponentId(i);
            }
        }
        
        throw new KeyNotFoundException("Component type not found");
    }

    internal int GetComponentSize(ComponentId id) => _componentSize[id.Index];

    internal int Count => _cursor;
}

public class DuplicateComponentException(string message) : Exception(message)
{
}