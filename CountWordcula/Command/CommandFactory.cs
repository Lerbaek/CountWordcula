using CountWordcula.Backend;
using CountWordcula.Backend.FileReader;
using CountWordcula.Backend.FileWriter;
using CountWordcula.Backend.Validate;
using GoCommando;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CountWordcula.Command;

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
      .AddSingleton<IFileReader, MemoryEfficientParallelFileReader>()
      .AddSingleton<IFileWriter, FileWriter>()
      .AddSingleton<IWordCountManager, WordCountManager>()
      .BuildServiceProvider();
  }

  public ICommand Create(Type commandType) => (ICommand)provider.GetRequiredService(commandType);

  public void Release(ICommand command)
  {
  }
}