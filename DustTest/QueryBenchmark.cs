namespace DustTest;

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Dust;
using Dust.Core;

[MemoryDiagnoser]
public class QueryBenchmark
{
    public int Amount = 1000000;

    private Dust _dust = null!;
    private Query _query = default;
    private ComponentId _transform = default;
    private ComponentId _velocity = default;

    [GlobalSetup]
    public void Setup()
    {
        _dust = new Dust();
        _transform = _dust.RegisterComponent<Transform>();
        _velocity = _dust.RegisterComponent<Velocity>();
        _dust.Initialize();
        _query = _dust.CreateQuery(_transform, _velocity);

        foreach (var i in Enumerable.Range(0, Amount))
        {
            var entity = _dust.CreateEntity(_transform, _velocity);
            ref var velocity = ref _dust.GetComponent<Velocity>(entity, _velocity);
            velocity.X = 1;
            velocity.Y = 1;
        }
    }

    [Benchmark]
    public void Query()
    {
        _dust.ForEach(_query, Update);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Update(Entity entity) {
        ref var transform = ref _dust.GetComponent<Transform>(entity, _transform);
        ref var velocity = ref _dust.GetComponent<Velocity>(entity, _velocity);
        transform.X += velocity.X;
        transform.Y += velocity.Y;
    }
}

public struct Transform
{
    public float X;
    public float Y;
}

public struct Velocity
{
    public float X;
    public float Y;
}
