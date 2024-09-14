namespace Dust;

using Core;

public class Dust
{
    private bool _initilized = false;
    private Entity entity;


    public Dust()
    {
    }

    public void Initialize()
    {
        if (_initilized)
        {
            return;
        }
        _initilized = true;
    }
}