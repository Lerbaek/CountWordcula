using CountWordcula.Backend;
using CountWordcula.Backend.FileReader;
using CountWordcula.Backend.FileWriter;
using CountWordcula.Backend.Validate;
using CountWordcula.Command;
using Microsoft.Extensions.DependencyInjection;
using Xunit.DependencyInjection;

namespace CountWordcula.Test;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services
      .AddSkippableFactSupport()
      .AddSingleton<IFileReader, MemoryEfficientParallelFileReader>()
      .AddSingleton<IFileWriter, FileWriter>()
      .AddSingleton<IWordCountManager, WordCountManager>()
      .AddSingleton<CountWordsCommand>()
      .AddSingleton<WordCountConfigurationValidator>()
      .AddSingleton<ExcludeFileValidator>()
      .AddSingleton<FluentFileReader>()
      .AddSingleton<MemoryEfficientFileReader>()
      .AddSingleton<MemoryEfficientParallelFileReader>();
  }
}