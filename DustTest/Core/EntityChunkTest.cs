namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class EntityChunkTest
{
    [TestMethod]
    public void TestCreate()
    {
        var chunk = new EntityChunk(2);
        var entity = chunk.Create();
        Assert.AreEqual(0, entity.Id);
        Assert.IsTrue(chunk.GetMask(entity).IsActive());

        var componentIndexes = chunk.GetComponentIndexes(entity);
        Assert.AreEqual(-1, componentIndexes[0]);

        chunk.GetMask(entity).Set(0);
        Assert.IsTrue(chunk.GetMask(entity).IsSet(0));
        chunk.GetMask(entity).Set(30);
        Assert.IsTrue(chunk.GetMask(entity).IsSet(30));
        Assert.ThrowsException<IndexOutOfRangeException>(() => chunk.GetMask(entity).Set(31));

        componentIndexes[0] = 42;
        var componentIndexes2 = chunk.GetComponentIndexes(entity);
        Assert.AreEqual(42, componentIndexes2[0]);

        var entity2 = chunk.Create();
        Assert.AreEqual(1, entity2.Id);
        Assert.IsTrue(chunk.GetMask(entity2).IsActive());
    }

    [TestMethod]
    public void TestRecycle()
    {
        var chunk = new EntityChunk(2);
        var entity = chunk.Create();
        chunk.GetMask(entity).Set(0);
        var componentIndexes = chunk.GetComponentIndexes(entity);
        componentIndexes[0] = 42;
        chunk.Recycle(entity);
        Assert.IsFalse(chunk.GetMask(entity).IsActive());

        var entity2 = chunk.Create();
        var componentIndexes2 = chunk.GetComponentIndexes(entity2);
        Assert.AreEqual(0, entity2.Id);
        Assert.IsTrue(chunk.GetMask(entity2).IsActive());
        Assert.AreEqual(-1, componentIndexes2[0]);
        Assert.IsFalse(chunk.GetMask(entity2).IsSet(0));
    }

    [TestMethod]
    public void TestCapacity()
    {
        var chunk = new EntityChunk(2);
        for (var i = 0; i < 8; i++)
        {
            var entity = chunk.Create();
            chunk.GetMask(entity).Set(0);
            var componentIndexes = chunk.GetComponentIndexes(entity);
            componentIndexes[0] = i;
        }
        Assert.AreEqual(8, chunk.Capacity);

        chunk.Create();
        Assert.AreEqual(16, chunk.Capacity);
        for (var i = 0; i < 8; i++)
        {
            var entity = new Entity(i);
            var componentIndexes = chunk.GetComponentIndexes(entity);
            Assert.AreEqual(i, componentIndexes[0]);
            Assert.IsTrue(chunk.GetMask(entity).IsSet(0));
        }
    }

    [TestMethod]
    public void TestRecycleCapacity()
    {
        var chunk = new EntityChunk(2);
        for (var i = 0; i < 8; i++)
        {
            chunk.Create();
        }
        Assert.AreEqual(8, chunk.Capacity);

        for (var i = 0; i < 8; i++)
        {
            var entity = new Entity(i);
            chunk.Recycle(entity);
        }
        Assert.AreEqual(8, chunk.Capacity);

        for (var i = 0; i < 8; i++)
        {
            chunk.Create();
        }
        Assert.AreEqual(8, chunk.Capacity);
    }
}