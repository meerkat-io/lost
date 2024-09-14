namespace DustTest.Core;

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
    public void TestGetIndex()
    {
        var registry = new ComponentRegistry();
        registry.Register<int>();
        Assert.AreEqual(0, registry.GetComponentId<int>().Index);
    }

    [TestMethod]
    public void TestGetIndexNotFound()
    {
        var registry = new ComponentRegistry();
        Assert.ThrowsException<KeyNotFoundException>(() => registry.GetComponentId<int>());
    }
}