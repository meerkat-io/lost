namespace Dust;

public class Dust
{
    private bool _isRunning = true;
    public static void Main()
    {
        var registry = new Core.ComponentRegistry();
        var index = registry.Register<int>();
        Console.WriteLine(index);
        index = registry.Register<float>();
        Console.WriteLine(index);
        Console.WriteLine(registry.GetIndex<int>());
        Console.WriteLine(registry.GetIndex<float>());
    }
}