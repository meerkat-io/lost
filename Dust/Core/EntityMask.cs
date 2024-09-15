namespace Dust.Core;

using System.Runtime.CompilerServices;

/// <summary>
/// Represents components bit mask in an Entity.
/// </summary>
/// Mask format: LOW 0bit entity activated HIGH 1-31bit component switches
internal struct EntityMask
{
    internal int Mask;

    internal EntityMask(bool activated)
    {
        Mask = activated ? 1 : 0;
    }

    internal EntityMask(bool activated, params ComponentId[] components)
    {
        Mask = activated ? 1 : 0;
        foreach (var component in components)
        {
            Mask |= 1 << component.Index + 1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Set(int index)
    {
        Mask |= 1 << index + 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Unset(int index)
    {
        Mask &= ~(1 << index + 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal readonly bool IsSet(int index)
    {
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