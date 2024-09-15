namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class EntityStorageTest
{
    [TestMethod]
    public void TestCreate()
    {
        var storage = new EntityStorage(2);
        var entity = storage.Create();
        Assert.AreEqual(0, entity.Id);
        Assert.IsTrue(storage.GetMask(entity).IsActive());
        Assert.AreEqual(-1, storage.GetComponentIndex(entity, new ComponentId(0)));

        storage.GetMask(entity).Set(0);
        Assert.IsTrue(storage.GetMask(entity).IsSet(0));
        storage.GetMask(entity).Set(30);
        Assert.IsTrue(storage.GetMask(entity).IsSet(30));

        storage.AddComponent(entity, new ComponentId(0), 42);
        Assert.AreEqual(42, storage.GetComponentIndex(entity, new ComponentId(0)));

        var entity2 = storage.Create();
        Assert.AreEqual(1, entity2.Id);
        Assert.IsTrue(storage.GetMask(entity2).IsActive());
    }

    [TestMethod]
    public void TestRecycle()
    {
        var storage = new EntityStorage(2);
        var entity = storage.Create();
        storage.GetMask(entity).Set(0);
        storage.AddComponent(entity, new ComponentId(0), 42);
        storage.Recycle(entity);
        Assert.IsFalse(storage.GetMask(entity).IsActive());

        var entity2 = storage.Create();
        Assert.AreEqual(0, entity2.Id);
        Assert.IsTrue(storage.GetMask(entity2).IsActive());
        Assert.AreEqual(-1, storage.GetComponentIndex(entity2, new ComponentId(0)));
        Assert.IsFalse(storage.GetMask(entity2).IsSet(0));
    }

    [TestMethod]
    public void TestCapacity()
    {
        var storage = new EntityStorage(2);
        for (var i = 0; i < 8; i++)
        {
            var entity = storage.Create();
            storage.GetMask(entity).Set(0);
            storage.AddComponent(entity, new ComponentId(0), i);
        }
        Assert.AreEqual(8, storage.Capacity);

        storage.Create();
        Assert.AreEqual(16, storage.Capacity);
        for (var i = 0; i < 8; i++)
        {
            var entity = new Entity(i);
            Assert.AreEqual(i, storage.GetComponentIndex(entity, new ComponentId(0)));
            Assert.IsTrue(storage.GetMask(entity).IsSet(0));
        }
    }

    [TestMethod]
    public void TestRecycleCapacity()
    {
        var storage = new EntityStorage(2);
        for (var i = 0; i < 8; i++)
        {
            storage.Create();
            Assert.AreEqual(i + 1, storage.CountEntities());
        }
        Assert.AreEqual(8, storage.Capacity);

        for (var i = 0; i < 8; i++)
        {
            var entity = new Entity(i);
            storage.Recycle(entity);
        }
        Assert.AreEqual(8, storage.Capacity);
        Assert.AreEqual(0, storage.CountEntities());

        for (var i = 0; i < 8; i++)
        {
            storage.Create();
        }
        Assert.AreEqual(8, storage.Capacity);
        Assert.AreEqual(8, storage.CountEntities());
    }

    [TestMethod]
    public void TestComponentOperation()
    {
        var storage = new EntityStorage(2);
        var entity = storage.Create();
        storage.AddComponent(entity, new ComponentId(0), 1);
        Assert.IsTrue(storage.GetMask(entity).IsSet(0));
        Assert.IsTrue(storage.HasComponent(entity, new ComponentId(0)));
        Assert.AreEqual(1, storage.GetComponentIndex(entity, new ComponentId(0)));

        storage.RemoveComponent(entity, new ComponentId(0));
        Assert.IsFalse(storage.GetMask(entity).IsSet(0));
        Assert.IsFalse(storage.HasComponent(entity, new ComponentId(0)));
        Assert.AreEqual(-1, storage.GetComponentIndex(entity, new ComponentId(0)));
    }

    [TestMethod]
    public void TestForEach()
    {
        var storage = new EntityStorage(2);
        var componentId0 = new ComponentId(0);
        var componentId1 = new ComponentId(1);
        var entity1 = storage.Create();
        storage.AddComponent(entity1, componentId0, 0);
        var entity2 = storage.Create();
        storage.AddComponent(entity2, componentId1, 0);
        var entity3 = storage.Create();
        storage.AddComponent(entity3, componentId0, 1);
        storage.AddComponent(entity3, componentId1, 1);
        var deactivated = storage.Create();
        storage.Recycle(deactivated);

        var count1 = 0;
        storage.ForEach(e => count1++);
        Assert.AreEqual(3, count1);

        var count2 = 0;
        storage.ForEach(new Query(componentId0), e => count2++);
        Assert.AreEqual(2, count2);

        var count3 = 0;
        storage.ForEach(new Query(componentId1), e => count3++);
        Assert.AreEqual(2, count3);
    }
}