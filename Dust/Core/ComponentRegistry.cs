namespace Dust.Core;

internal class ComponentRegistry
{
    private const int _maxComponents = 31;
    private int _cursor = 0;
    private readonly int[] _registry = new int[_maxComponents];

    internal int Register<T>() where T : struct
    {
        if (_cursor == _maxComponents)
        {
            throw new IndexOutOfRangeException("Component registry is full");
        }
        if (GetIndex<T>() != -1)
        {
            throw new DuplicateComponentException("Component type already registered");
        }
        var index = _cursor++;
        _registry[index] = typeof(T).GetHashCode();
        return index;
    }

    internal int GetIndex<T>() where T : struct
    {
        var hashCode = typeof(T).GetHashCode();
        for (var i = 0; i < _cursor; i++)
        {
            if (_registry[i] == hashCode)
            {
                return i;
            }
        }
        return -1;
    }
}

public class DuplicateComponentException(string message) : Exception(message)
{
}