namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class ComponentRegistryTest
{
    [TestMethod]
    public void TestRegister()
    {
        var registry = new ComponentRegistry();
        var index = registry.Register<int>();
        Assert.AreEqual(0, index);
        index = registry.Register<float>();
        Assert.AreEqual(1, index);
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
        Assert.AreEqual(0, registry.GetIndex<int>());
    }

    [TestMethod]
    public void TestGetIndexNotFound()
    {
        var registry = new ComponentRegistry();
        Assert.ThrowsException<KeyNotFoundException>(() => registry.GetIndex<int>());
    }
}