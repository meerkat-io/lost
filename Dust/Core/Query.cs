namespace Dust.Core;

public struct Query
{
    internal EntityMask _mask;
    
    internal Query(params ComponentId[] components)
    {
        _mask = new EntityMask(true, components);
    }
}