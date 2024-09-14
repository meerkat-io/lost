namespace Dust.Core;

using System.Runtime.CompilerServices;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Set(int index)
    {
        if (index > 30 || index < 0)
        {
            throw new IndexOutOfRangeException("Component bit index out of range");
        }
        Mask |= 1 << index + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Unset(int index)
    {
        if (index > 30 || index < 0)
        {
            throw new IndexOutOfRangeException("Component bit index out of range");
        }
        Mask &= ~(1 << index + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly bool IsSet(int index)
    {
        if (index > 30 || index < 0)
        {
            throw new IndexOutOfRangeException("Component bit index out of range");
        }
        return (Mask & 1 << index + 1) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Activate()
    {
        Mask |= 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Deactivate()
    {
        Mask &= ~1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly bool IsActive()
    {
        return (Mask & 1) != 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly bool Contains(EntityMask other)
    {
        return (Mask & other.Mask) == other.Mask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Clear()
    {
        Mask = 0;
    }
}