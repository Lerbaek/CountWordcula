using CountWordcula.Backend;
using CountWordcula.Backend.FileReader;
using CountWordcula.Backend.FileWriter;
using CountWordcula.Validate;
using GoCommando;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static CountWordcula.Command.CountWordsParameterValues;

namespace CountWordcula.Command;

[Command(CountWordsCommandName)]
[Description(CountWordsDescription)]
public class CountWords : ICommand
{
  private readonly IServiceProvider provider;

  public CountWords()
  {
    provider = new ServiceCollection()
      .AddLogging(
        builder => builder.AddSerilog(
          new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()))
      .AddSingleton<CountWordsValidator>()
      .AddSingleton<ExcludeFileValidator>()
      .AddSingleton<IFileReader, MemoryEfficientParallelFileReader>()
      .AddSingleton<IFileWriter, FileWriter>()
      .AddSingleton<IWordCountManager, WordCountManager>()
      .BuildServiceProvider();
  }

  [Parameter(InputPathName, InputPathShortName)]
  [Description(InputPathDescription)]
  public string InputPath { get; set; } = null!;

  [Parameter(OutputPathName, OutputPathShortName)]
  [Description(OutputPathDescription)]
  public string OutputPath { get; set; } = null!;

  [Parameter(ExtensionName, ExtensionShortName, ExtensionOptional, ExtensionDefaultValue)]
  [Description(ExtensionDescription)]
  public string Extension { get; set; } = ExtensionDefaultValue;

  [Parameter(ForceName, ForceShortName, ForceOptional)]
  [Description(ForceDescription)]
  public bool Force { get; set; }

  public void Run() => RunAsync().Wait();

  public async Task RunAsync()
  {
    SanitizeInput();
    await ValidateInput();
    var wordCountManager = provider.GetRequiredService<IWordCountManager>();
    var excludedWords = await wordCountManager.GetExcludedWordsAsync(InputPath);
    await ValidateExcludedWords(excludedWords);
    await wordCountManager.ExecuteAsync(InputPath, OutputPath, excludedWords);
  }

  private void SanitizeInput() => Extension = Extension.TrimStart('.');

  private async Task ValidateInput()
  {
    var validator = provider
      .GetRequiredService<CountWordsValidator>();
    var validation = await validator
      .ValidateAsync(this);

    if (!validation.IsValid)
      Environment.Exit(11);
  }

  private async Task ValidateExcludedWords(string[] words)
  {
    var validator = provider.GetRequiredService<ExcludeFileValidator>();
    var validationResult = await validator.ValidateAsync(words);
    if (!validationResult.IsValid)
      Environment.Exit(11);
  }
}