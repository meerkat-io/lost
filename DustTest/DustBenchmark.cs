namespace DustTest;

using BenchmarkDotNet.Running;

public class DustBenchmark
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<QueryBenchmark>();
    }
}