namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class EntityChunkTest
{
    [TestMethod]
    public void TestCreateEntity()
    {
        var chunk = new EntityChunk(2);
        var entity = chunk.Create();
        Assert.AreEqual(0, entity.Id);
        Assert.AreEqual(1, chunk.GetMask(entity));

        var componentIndexes = chunk.GetComponentIndexes(entity);
        Assert.AreEqual(-1, componentIndexes[0]);

        componentIndexes[0] = 42;
        var componentIndexes2 = chunk.GetComponentIndexes(entity);
        Assert.AreEqual(42, componentIndexes2[0]);

        var entity2 = chunk.Create();
        Assert.AreEqual(1, entity2.Id);
        Assert.AreEqual(1, chunk.GetMask(entity2));
    }

    [TestMethod]
    public void TestRecycleEntity()
    {
        var chunk = new EntityChunk(2);
        var entity = chunk.Create();
        Assert.AreEqual(0, entity.Id);
        Assert.AreEqual(1, chunk.GetMask(entity));

        var componentIndexes = chunk.GetComponentIndexes(entity);
        componentIndexes[0] = 42;
        chunk.Recycle(entity);

        var entity2 = chunk.Create();
        var componentIndexes2 = chunk.GetComponentIndexes(entity2);
        Assert.AreEqual(0, entity2.Id);
        Assert.AreEqual(1, chunk.GetMask(entity2));
        Assert.AreEqual(-1, componentIndexes2[0]);
    }
}