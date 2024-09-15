namespace DustTest;

using Dust;

[TestClass]
public class DustTest
{
    [TestMethod]
    public void TestCreateEntity()
    {
        var dust = new Dust();
        var componentId1 = dust.RegisterComponent<int>();
        var componentId2 = dust.RegisterComponent<float>();
        dust.Initialize();

        var entity = dust.CreateEntity(componentId1, componentId2);
        Assert.IsTrue(dust.HasComponent(entity, componentId1));
        Assert.IsTrue(dust.HasComponent(entity, componentId2));

        ref var component1 = ref dust.GetComponent<int>(entity, componentId1);
        component1 = 42;
        Assert.AreEqual(42, dust.GetComponent<int>(entity, componentId1));

        ref var component2 = ref dust.GetComponent<float>(entity, componentId2);
        component2 = 3.14f;
        Assert.AreEqual(3.14f, dust.GetComponent<float>(entity, componentId2));
    }

    [TestMethod]
    public void TestRemoveEntity()
    {
        var dust = new Dust();
        var componentId1 = dust.RegisterComponent<int>();
        var componentId2 = dust.RegisterComponent<float>();
        dust.Initialize();

        var entity = dust.CreateEntity(componentId1, componentId2);
        Assert.AreEqual(1, dust.CountEntities());
        dust.RemoveEntity(entity);
        Assert.ThrowsException<InvalidOperationException>(() => dust.GetComponent<int>(entity, componentId1));
        Assert.AreEqual(0, dust.CountEntities());
    }

    [TestMethod]
    public void TestRemoveEntityTwice()
    {
        var dust = new Dust();
        var componentId1 = dust.RegisterComponent<int>();
        var componentId2 = dust.RegisterComponent<float>();
        dust.Initialize();

        var entity = dust.CreateEntity(componentId1, componentId2);
        Assert.AreEqual(1, dust.CountEntities());
        dust.RemoveEntity(entity);
        Assert.ThrowsException<InvalidOperationException>(() => dust.RemoveEntity(entity));
        Assert.AreEqual(0, dust.CountEntities());
    }
}