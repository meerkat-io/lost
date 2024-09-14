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

        var componentIndexes = storage.GetComponents(entity);
        Assert.AreEqual(-1, componentIndexes[0]);

        storage.GetMask(entity).Set(0);
        Assert.IsTrue(storage.GetMask(entity).IsSet(0));
        storage.GetMask(entity).Set(30);
        Assert.IsTrue(storage.GetMask(entity).IsSet(30));

        componentIndexes[0] = 42;
        var componentIndexes2 = storage.GetComponents(entity);
        Assert.AreEqual(42, componentIndexes2[0]);

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
        var components = storage.GetComponents(entity);
        components[0] = 42;
        storage.Recycle(entity);
        Assert.IsFalse(storage.GetMask(entity).IsActive());

        var entity2 = storage.Create();
        var components2 = storage.GetComponents(entity2);
        Assert.AreEqual(0, entity2.Id);
        Assert.IsTrue(storage.GetMask(entity2).IsActive());
        Assert.AreEqual(-1, components2[0]);
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
            var components = storage.GetComponents(entity);
            components[0] = i;
        }
        Assert.AreEqual(8, storage.Capacity);

        storage.Create();
        Assert.AreEqual(16, storage.Capacity);
        for (var i = 0; i < 8; i++)
        {
            var entity = new Entity(i);
            var components = storage.GetComponents(entity);
            Assert.AreEqual(i, components[0]);
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
        }
        Assert.AreEqual(8, storage.Capacity);

        for (var i = 0; i < 8; i++)
        {
            var entity = new Entity(i);
            storage.Recycle(entity);
        }
        Assert.AreEqual(8, storage.Capacity);

        for (var i = 0; i < 8; i++)
        {
            storage.Create();
        }
        Assert.AreEqual(8, storage.Capacity);
    }
}