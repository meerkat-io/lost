namespace DustTest.Core;

using Dust.Core;

[TestClass]
public class EntityMaskTest
{
    [TestMethod]
    public void TestActive()
    {
        var mask = new EntityMask(0);
        Assert.IsFalse(mask.IsActive());

        mask.Activate();
        Assert.IsTrue(mask.IsActive());

        mask.Deactivate();
        Assert.IsFalse(mask.IsActive());
    }

    [TestMethod]
    public void TestSet()
    {
        var mask = new EntityMask(0);
        Assert.IsFalse(mask.IsSet(0));

        mask.Set(0);
        Assert.IsTrue(mask.IsSet(0));

        mask.Unset(0);
        Assert.IsFalse(mask.IsSet(0));
    }

    [TestMethod]
    public void TestContains()
    {
        var mask1 = new EntityMask(0);
        var mask2 = new EntityMask(0);
        Assert.IsTrue(mask1.Contains(mask2));
        Assert.IsTrue(mask2.Contains(mask1));

        mask1.Set(0);
        Assert.IsTrue(mask1.Contains(mask2));
        Assert.IsFalse(mask2.Contains(mask1));

        mask2.Set(0);
        Assert.IsTrue(mask1.Contains(mask2));
        Assert.IsTrue(mask2.Contains(mask1));

        mask1.Set(1);
        Assert.IsTrue(mask1.Contains(mask2));
        Assert.IsFalse(mask2.Contains(mask1));

        mask2.Set(2);
        Assert.IsFalse(mask1.Contains(mask2));
        Assert.IsFalse(mask2.Contains(mask1));
    }

    [TestMethod]
    public void TestClear()
    {
        var mask = new EntityMask(0);
        mask.Activate();
        mask.Set(0);
        mask.Set(1);
        mask.Set(2);
        mask.Clear();
        Assert.IsFalse(mask.IsActive());
        Assert.IsFalse(mask.IsSet(0));
        Assert.IsFalse(mask.IsSet(1));
        Assert.IsFalse(mask.IsSet(2));
    }
}