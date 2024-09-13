namespace Dust.Core;

public class Query
{
    internal int _componentMask;
    internal Query(int componentMask)
    {
        _componentMask = componentMask;
    }
}