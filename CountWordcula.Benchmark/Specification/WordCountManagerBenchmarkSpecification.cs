using BenchmarkDotNet.Attributes;
using CountWordcula.Backend;
using CountWordcula.Backend.FileWrite;
using CountWordcula.Backend.Validate;
using Microsoft.Extensions.Logging;
using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Benchmark.Specification;

public class WordCountManagerBenchmarkSpecification : BenchmarkSpecificationBase

{
  [Benchmark]
  public async Task<bool> WordCountManagerBenchmark()
  {
    return await new WordCountManager(
        FileReader,
        new FileWriter(),
        new WordCountConfigurationValidator(EmptyLogger<WordCountConfigurationValidator>()),
        new ExcludeFileValidator(EmptyLogger<ExcludeFileValidator>()),
        EmptyLogger<WordCountManager>())
      .RunAsync(
        SampleInputDirectoryName,
        "txt",
        ResultDirectoryName,
        true);
  }
  public static Logger<T> EmptyLogger<T>() => new(LoggerFactory.Create(c => { } ));
}