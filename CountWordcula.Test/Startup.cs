using CountWordcula.Read;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace CountWordcula.Test;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddTransient<ICountWords, CountWords>();
  }

  public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor) =>
    loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));
}