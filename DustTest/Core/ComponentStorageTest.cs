namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class ComponentStorageTest
{
    [TestMethod]
    public void TestUpdateComponent()
    {
        var storage = new ComponentStorage(typeof(int));
        var index = storage.Create();
        Assert.AreEqual(0, storage.GetItem<int>(index));

        ref var component = ref storage.GetItem<int>(index);
        component = 42;
        Assert.AreEqual(42, storage.GetItem<int>(index));
    }

    [TestMethod]
    public void TestRecycle()
    {
        var storage = new ComponentStorage(typeof(int));
        var index = storage.Create();
        ref var component = ref storage.GetItem<int>(index);
        component = 42;
        storage.Recycle(index);
        var newIndex = storage.Create();
        Assert.AreEqual(0, storage.GetItem<int>(newIndex));
        Assert.AreEqual(index, newIndex);
    }

    [TestMethod]
    public void TestCapacity()
    {
        var storage = new ComponentStorage(typeof(int));
        for (var i = 0; i < 8; i++)
        {
            var index = storage.Create();
            ref var component = ref storage.GetItem<int>(index);
            component = i;
        }
        Assert.AreEqual(8, storage.Capacity);

        var newIndex = storage.Create();
        for (var i = 0; i < 8; i++)
        {
            Assert.AreEqual(i, storage.GetItem<int>(i));
        }
        Assert.AreEqual(16, storage.Capacity);
    }

    [TestMethod]
    public void TestRecycleCapacity()
    {
        var storage = new ComponentStorage(typeof(int));
        for (var i = 0; i < 8; i++)
        {
            storage.Create();
        }
        Assert.AreEqual(8, storage.Capacity);

        for (var i = 0; i < 8; i++)
        {
            storage.Recycle(i);
        }
        Assert.AreEqual(8, storage.Capacity);

        for (var i = 0; i < 8; i++)
        {
            storage.Create();
        }
        Assert.AreEqual(8, storage.Capacity);
    }
}