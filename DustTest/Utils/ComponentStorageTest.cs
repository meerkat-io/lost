namespace DustTest.Utils;

using Dust.Utils;

[TestClass]
public class ComponentStorageTest
{
    [TestMethod]
    public void TestValueUpdate()
    {
        var storage = new ComponentStorage<int>();
        var index = storage.Create();
        Assert.AreEqual(0, storage[index]);
        ref var value = ref storage[index];
        value = 42;
        Assert.AreEqual(42, storage[index]);
    }

    [TestMethod]
    public void TestRecycle()
    {
        var storage = new ComponentStorage<int>();
        var index = storage.Create();
        storage[index] = 42;
        storage.Recycle(index);
        Assert.AreEqual(0, storage[index]);
        var newIndex = storage.Create();
        Assert.AreEqual(index, newIndex);
    }

    [TestMethod]
    public void TestCapacity()
    {
        var storage = new ComponentStorage<int>();
        for (var i = 0; i < 8; i++)
        {
            storage.Create();
        }
        Assert.AreEqual(8, storage.Capacity);
        storage.Create();
        Assert.AreEqual(16, storage.Capacity);
    }

    [TestMethod]
    public void TestRecycleCapacity()
    {
        var storage = new ComponentStorage<int>();
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