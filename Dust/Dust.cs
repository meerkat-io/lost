namespace Dust;

using System.Runtime.CompilerServices;
using Core;

public class Dust
{
    private bool _initilized = false;
    private readonly ComponentRegistry _registry = new();
    private EntityStorage? _entities = null;
    private ComponentStorage[]? _components = null;

    public ComponentId RegisterComponent<T>() where T : struct
    {
        if (_initilized)
        {
            throw new InvalidOperationException("Cannot register components after Dust has been initialized.");
        }
        return _registry.Register<T>();
    }

    public void Initialize()
    {
        if (_initilized)
        {
            throw new InvalidOperationException("Dust has already been initialized.");
        }
        _initilized = true;

        var count = _registry.Count;
        _entities = new EntityStorage(count);
        _components = new ComponentStorage[count];
        for (var i = 0; i < count; i++)
        {
            _components[i] = new ComponentStorage(_registry.GetComponentSize(new ComponentId(i)));
        }
    }

    public Entity CreateEntity(params ComponentId[] components)
    {
        CheckInitialized();

        var entity = _entities!.Create();
        foreach (var component in components)
        {
            //_entities!.AddComponent(entity, componentId);
        }
        return entity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckInitialized()
    {
        if (!_initilized)
        {
            throw new InvalidOperationException("Dust has not been initialized.");
        }
    }
}