namespace Dust.Core;

public struct Entity
{
    internal int Id;

    internal Entity(int id)
    {
        Id = id;
    }
}

public delegate void EntityHandler(Entity entity, Dust dust);