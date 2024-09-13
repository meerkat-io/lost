namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class ComponentChunkTest
{
    [TestMethod]
    public void TestUpdateComponent()
    {
        var storage = new ComponentChunk<int>();
        var index = storage.Create();
        Assert.AreEqual(0, storage[index]);

        ref var component = ref storage[index];
        component = 42;
        Assert.AreEqual(42, storage[index]);
    }

    [TestMethod]
    public void TestRecycle()
    {
        var storage = new ComponentChunk<int>();
        var index = storage.Create();
        storage[index] = 42;
        storage.Recycle(index);
        var newIndex = storage.Create();
        Assert.AreEqual(0, storage[newIndex]);
        Assert.AreEqual(index, newIndex);
    }

    [TestMethod]
    public void TestCapacity()
    {
        var storage = new ComponentChunk<int>();
        for (var i = 0; i < 8; i++)
        {
            var index = storage.Create();
            storage[index] = i;
        }
        Assert.AreEqual(8, storage.Capacity);

        var newIndex = storage.Create();
        for (var i = 0; i < 8; i++)
        {
            Assert.AreEqual(i, storage[i]);
        }
        Assert.AreEqual(16, storage.Capacity);
    }

    [TestMethod]
    public void TestRecycleCapacity()
    {
        var storage = new ComponentChunk<int>();
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