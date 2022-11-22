using CountWordcula.Backend;
using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.FileWrite;
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
      .AddSingleton<IFileReader, ConcurrentLinesFileReader>()
      .AddSingleton<IFileWriter, FileWriter>()
      .AddSingleton<IWordCountManager, WordCountManager>()
      .AddSingleton<CountWordsCommand>()
      .AddSingleton<WordCountConfigurationValidator>()
      .AddSingleton<ExcludeFileValidator>()
      .AddSingleton<FluentFileReader>()
      .AddSingleton<MemoryEfficientFileReader>()
      .AddSingleton<ConcurrentLinesFileReader>();
  }
}