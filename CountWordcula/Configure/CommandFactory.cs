using CountWordcula.Backend;
using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.FileWrite;
using CountWordcula.Backend.Validate;
using CountWordcula.Command;
using GoCommando;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CountWordcula.Configure;

public class CommandFactory : ICommandFactory
{
  private readonly IServiceProvider provider;

  public CommandFactory()
  {
    provider = new ServiceCollection()
      .AddLogging(
        builder => builder.AddSerilog(
          new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()))
      .AddSingleton<CountWordsCommand>()
      .AddSingleton<WordCountConfigurationValidator>()
      .AddSingleton<ExcludeFileValidator>()
      .AddSingleton<IFileReader, ConcurrentLinesFileReader>()
      .AddSingleton<IFileWriter, FileWriter>()
      .AddSingleton<IWordCountManager, WordCountManager>()
      .BuildServiceProvider();
  }

  public ICommand Create(Type commandType) => (ICommand)provider.GetRequiredService(commandType);

  public void Release(ICommand command)
  {
  }
}