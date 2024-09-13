namespace Dust.Core;

public struct Entity
{
    internal int Id;
    internal int ComponentMask;

    internal Entity(int id, int componentMask)
    {
        Id = id;
        ComponentMask = componentMask;
    }
}