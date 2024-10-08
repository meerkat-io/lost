namespace Dust;

using System.Runtime.CompilerServices;
using Core;

public class Dust
{
    private bool _initilized = false;
    private readonly ComponentRegistry _registry = new();
    private EntityStorage _entities = null!;
    private ComponentStorage[] _components = null!;

    public ComponentId RegisterComponent<T>() where T : struct
    {
        if (_initilized)
        {
            throw new InvalidOperationException("Cannot register components after Dust has been initialized.");
        }
        return _registry.Register<T>();
    }

    public ComponentId GetComponentId<T>() where T : struct
    {
        return _registry.GetComponentId<T>();
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

        var entity = _entities.Create();
        foreach (var component in components)
        {
            var index = _components[component.Index].Create();
            _entities.AddComponent(entity, component, index);
        }
        return entity;
    }

    public void RemoveEntity(Entity entity)
    {
        CheckInitialized();

        if (!_entities.IsActive(entity))
        {
            throw new InvalidOperationException("Entity does not exist or recycled.");
        }
        EntityMask mask = _entities.GetMask(entity);
        for (var i = 0; i < _registry.Count; i++)
        {
            if (mask.IsSet(i))
            {
                _components[i].Recycle(_entities.GetComponentIndex(entity, new ComponentId(i)));
            }
        }
        _entities.Recycle(entity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddComponent(Entity entity, ComponentId component)
    {
        CheckInitialized();

        if (!_entities.IsActive(entity))
        {
            throw new InvalidOperationException("Entity does not exist or recycled.");
        }
        if (_entities.HasComponent(entity, component))
        {
            throw new InvalidOperationException("Component has not been registered.");
        }
        var index = _components[component.Index].Create();
        _entities.AddComponent(entity, component, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool HasComponent(Entity entity, ComponentId componentId)
    {
        CheckInitialized();

        if (!_entities.IsActive(entity))
        {
            throw new InvalidOperationException("Entity does not exist or recycled.");
        }
        return _entities.HasComponent(entity, componentId);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T GetComponent<T>(Entity entity, ComponentId componentId) where T : struct
    {
        CheckInitialized();

        if (!_entities.IsActive(entity))
        {
            throw new InvalidOperationException("Entity does not exist or recycled.");
        }
        var componentIndex = _entities.GetComponentIndex(entity, componentId);
        if (componentIndex == -1)
        {
            throw new InvalidOperationException("Entity does not have component.");
        }
        return ref _components[componentId.Index].GetItem<T>(componentIndex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveComponent(Entity entity, ComponentId component)
    {
        CheckInitialized();

        if (!_entities.IsActive(entity))
        {
            throw new InvalidOperationException("Entity does not exist or recycled.");
        }
        if (!_entities.HasComponent(entity, component))
        {
            throw new InvalidOperationException("Entity does not have component.");
        }
        var componentIndex = _entities.GetComponentIndex(entity, component);
        _components[component.Index].Recycle(componentIndex);
        _entities.RemoveComponent(entity, component);
    }

    public Query CreateQuery(params ComponentId[] components)
    {
        CheckInitialized();

        return new Query(components);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ForEach(Action<Entity> action)
    {
        CheckInitialized();

        _entities.ForEach(action);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ForEach(Query query, Action<Entity> action)
    {
        CheckInitialized();

        _entities.ForEach(query, action);
    }

    public int CountEntities()
    {
        CheckInitialized();

        return _entities.CountEntities();
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