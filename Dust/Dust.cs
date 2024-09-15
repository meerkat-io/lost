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
            var index = _components![component.Index].Create();
            _entities!.AddComponent(entity, component, index);
        }
        return entity;
    }

    //RemoveEntity

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent(Entity entity, ComponentId component)
    {
        CheckInitialized();

        if (_entities!.HasComponent(entity, component))
        {
            throw new InvalidOperationException("Component has not been registered.");
        }
        var index = _components![component.Index].Create();
        _entities!.AddComponent(entity, component, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponent<T>(Entity entity, ComponentId componentId) where T : struct
    {
        CheckInitialized();

        var componentIndex = _entities!.GetComponentIndex(entity, componentId);
        if (componentIndex == -1)
        {
            throw new InvalidOperationException("Entity does not have component.");
        }
        return ref _components![componentId.Index].GetItem<T>(componentIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent(Entity entity, ComponentId component)
    {
        CheckInitialized();

        if (!_entities!.HasComponent(entity, component))
        {
            throw new InvalidOperationException("Entity does not have component.");
        }
        var componentIndex = _entities!.GetComponentIndex(entity, component);
        _components![component.Index].Recycle(componentIndex);
        _entities!.RemoveComponent(entity, component);
    }

    //CreateQuery
    //DisposeQuery
    //ForEach (Query, Action<Entity>)

    //ForEach all entities
    //CountEntities

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckInitialized()
    {
        if (!_initilized)
        {
            throw new InvalidOperationException("Dust has not been initialized.");
        }
    }
}