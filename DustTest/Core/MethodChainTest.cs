namespace DustTest.Core;

[TestClass]
public class MethodChainTest
{
    [TestMethod]
    public void TestMethodChain()
    {
        //var chain = new MethodChain();
        //chain.Register<int, float>();
    }
}

public struct MethodChain
{
    public MethodChain()
    {
        
    }
/*
    public ref MethodChain Register<T>()
    {
        Console.WriteLine(typeof(T));
        return ref this;
    }*/
}