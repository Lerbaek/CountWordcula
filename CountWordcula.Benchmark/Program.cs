using BenchmarkDotNet.Running;

namespace CountWordcula.Benchmark
{
  internal class Program
  {
    static void Main(string[] args)
    {
      BenchmarkRunner.Run<BenchmarkSpecification>();
    }
  }
}