using CountWordcula.Backend.FileReader;
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
      .AddTransient<CountWords>()
      .AddSingleton<FluentFileReader>()
      .AddSingleton<MemoryEfficientFileReader>()
      .AddSingleton<MemoryEfficientParallelFileReader>();
  }

  public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor) =>
    loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));
}