using BenchmarkDotNet.Running;
using CountWordcula.Benchmark.Specification;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Benchmark
{
  internal class Program
  {
    static void Main()
    {
      BenchmarkRunner.Run<
        //FileReaderBenchmarkSpecification
        WordCountManagerBenchmarkSpecification
      >();
    }

  }
}