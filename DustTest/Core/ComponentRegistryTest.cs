namespace DustTest.Core;

using System.Runtime.CompilerServices;
using Dust.Core;

[TestClass]
public class ComponentRegistryTest
{
    [TestMethod]
    public void TestRegister()
    {
        var registry = new ComponentRegistry();
        var componentId = registry.Register<int>();
        Assert.AreEqual(0, componentId.Index);
        componentId = registry.Register<float>();
        Assert.AreEqual(1, componentId.Index);
    }

    [TestMethod]
    public void TestDuplicateRegister()
    {
        var registry = new ComponentRegistry();
        registry.Register<int>();
        Assert.ThrowsException<DuplicateComponentException>(() => registry.Register<int>());
    }

    [TestMethod]
    public void TestGetComponentId()
    {
        var registry = new ComponentRegistry();
        registry.Register<int>();
        Assert.AreEqual(0, registry.GetComponentId<int>().Index);
    }

    [TestMethod]
    public void TestGetComponentIdNotFound()
    {
        var registry = new ComponentRegistry();
        Assert.ThrowsException<KeyNotFoundException>(() => registry.GetComponentId<int>());
    }

    [TestMethod]
    public void TestCount()
    {
        var registry = new ComponentRegistry();
        Assert.AreEqual(0, registry.Count);
        registry.Register<int>();
        Assert.AreEqual(1, registry.Count);
        registry.Register<float>();
        Assert.AreEqual(2, registry.Count);
    }

    [TestMethod]
    public void TestGetComponentSize()
    {
        var registry = new ComponentRegistry();
        registry.Register<int>();
        Assert.AreEqual(Unsafe.SizeOf<int>(), registry.GetComponentSize(new ComponentId(0)));
    }
}