namespace Dust.Core;

/// <summary>
/// Represents components bit mask in an Entity.
/// </summary>
/// Mask format: LOW 0bit entity activated HIGH 1-31bit component switches
internal struct EntityMask
{
    internal int Mask;

    internal EntityMask(int mask)
    {
        Mask = mask;
    }

    internal void Set(int index)
    {
        if (index > 30 || index < 0)
        {
            throw new IndexOutOfRangeException("Component bit index out of range");
        }
        Mask |= 1 << index + 1;
    }

    internal void Unset(int index)
    {
        if (index > 30 || index < 0)
        {
            throw new IndexOutOfRangeException("Component bit index out of range");
        }
        Mask &= ~(1 << index + 1);
    }

    internal readonly bool IsSet(int index)
    {
        if (index > 30 || index < 0)
        {
            throw new IndexOutOfRangeException("Component bit index out of range");
        }
        return (Mask & 1 << index + 1) != 0;
    }

    internal void Activate()
    {
        Mask |= 1;
    }

    internal void Deactivate()
    {
        Mask &= ~1;
    }

    internal readonly bool IsActive()
    {
        return (Mask & 1) != 0;
    }

    internal readonly bool Contains(EntityMask other)
    {
        return (Mask & other.Mask) == other.Mask;
    }

    internal void Clear()
    {
        Mask = 0;
    }
}