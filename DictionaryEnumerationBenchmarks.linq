<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

void Main()
{
    var summary = BenchmarkRunner.Run<DictionaryAccessBenchmarks>();
}

[MemoryDiagnoser]
public class DictionaryAccessBenchmarks
{
    private readonly int n = 10_000_000;
    
    [Benchmark]
    public void AccessViaDictionary()
    {
        var dict = new Dictionary<int, int>();

        for (var i = 0; i < n; i++)
        {
            foreach (var item in dict)
            {

            }
        }
    }
    
    [Benchmark]
    public void AccessViaIDictionary()
    {
        var dict = (IDictionary<int, int>) new Dictionary<int, int>();

        for (var i = 0; i < n; i++)
        {
            foreach (var item in dict)
            {

            }
        }
    }
}