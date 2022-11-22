using CountWordcula.Backend;
using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.FileWrite;
using CountWordcula.Backend.Validate;
using CountWordcula.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

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

  public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor) =>
    loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));
}