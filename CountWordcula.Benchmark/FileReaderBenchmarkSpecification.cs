using BenchmarkDotNet.Attributes;
using CountWordcula.Backend;
using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.FileWrite;
using CountWordcula.Backend.Validate;
using Microsoft.Extensions.Logging;
using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Benchmark;

[MemoryDiagnoser]
public class FileReaderBenchmarkSpecification : BenchmarkSpecificationBase

{
  private string FilePath =>
    Path.Combine(
      Environment.CurrentDirectory,
      SampleInputDirectoryName,
      $"Sample_{WordCount}.txt");

  [Params(200, 2000, 5000, 10000, 10000000)]
  public int WordCount { get; set; }


  [Benchmark]
  public async Task<WordCount> FileReaderBenchmark() => await FileReader.GetWordCountAsync(FilePath);
}

//[MemoryDiagnoser]
public class BenchmarkSpecificationBase
{
  [Params(typeof(FluentFileReader), typeof(MemoryEfficientFileReader), typeof(ConcurrentLinesFileReader)/*, typeof(ConcurrentBlocksFileReader)*/)]
  public Type FileReaderType { get; set; } = null!;

  protected IFileReader FileReader =>
    (IFileReader)FileReaderType.GetConstructor(Type.EmptyTypes)!
      .Invoke(Type.EmptyTypes);

}

public class WordCountManagerBenchmarkSpecification : BenchmarkSpecificationBase

{
  [Benchmark]
  public async Task<bool> WordCountManagerBenchmark()
  {
    return await new WordCountManager(
        FileReader,
        new FileWriter(),
        new WordCountConfigurationValidator(
          EmptyLogger<WordCountConfigurationValidator>()),
        new ExcludeFileValidator(EmptyLogger<ExcludeFileValidator>()),
        EmptyLogger<WordCountManager>())
      .RunAsync(
        SampleInputDirectoryName,
        "txt",
        ResultDirectoryName,
        true);
  }

  private static Logger<T> EmptyLogger<T>()
  {
    return new Logger<T>(LoggerFactory.Create(_ => { }));
  }
}