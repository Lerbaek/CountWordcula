using BenchmarkDotNet.Attributes;
using CountWordcula.Backend;
using CountWordcula.Backend.Registry;

namespace CountWordcula.Benchmark.Specification;

[MemoryDiagnoser]
public class FileReaderBenchmarkSpecification : BenchmarkSpecificationBase

{
  private string FilePath =>
    Path.Combine(
      Environment.CurrentDirectory,
      ConfigurationRegistry.SampleInputDirectoryName,
      $"Sample_{WordCount}.txt");

  [Params(200, 2000, 5000, 10000, 10000000)]
  public int WordCount { get; set; }


  [Benchmark]
  public async Task<WordCount> FileReaderBenchmark() => await FileReader.GetWordCountAsync(FilePath);
}