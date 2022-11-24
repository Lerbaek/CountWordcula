using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using static CountWordcula.Profiling.Program;

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