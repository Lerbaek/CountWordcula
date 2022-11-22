using BenchmarkDotNet.Attributes;
using CountWordcula.Backend;
using CountWordcula.Backend.FileRead;
using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Benchmark;

[MemoryDiagnoser]
public class BenchmarkSpecification

{
  [Params(typeof(FluentFileReader), typeof(MemoryEfficientFileReader), typeof(ConcurrentLinesFileReader))]
  public Type FileReaderType { get; set; } = null!;

  private IFileReader FileReader =>
    (IFileReader)FileReaderType.GetConstructor(Type.EmptyTypes)!
      .Invoke(Type.EmptyTypes);

  private string FilePath =>
    Path.Combine(
      Environment.CurrentDirectory,
      SampleInputDirectoryName,
      $"Sample_{WordCount}.txt");

  [Params(
    200
    ,
    2000
    ,
    5000
    ,
    10000
    )]
  public int WordCount { get; set; }


  [Benchmark]
  public async Task<WordCount> FileReaderBenchmark() =>
    await FileReader.GetWordCountAsync(FilePath);
}