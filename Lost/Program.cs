namespace Lost;

using Dust;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var dust = new Dust();
        dust.RegisterComponent<int>();
    }
}
