using CountWordcula.Backend;
using CountWordcula.Backend.FileReader;
using CountWordcula.Backend.FileWriter;
using CountWordcula.Validate;
using GoCommando;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using static CountWordcula.Command.CountWordsParameterValues;

namespace CountWordcula.Command;

[Command("count-words")]
[Description("Read data from multiple files in a given directory.")]
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

  [Parameter(SegregateName, SegregateShortName, SegregateOptional)]
  [Description(SegregateDescription)]
  public bool Segregate { get; set; }

  [Parameter(ForceName, ForceShortName, ForceOptional)]
  [Description(ForceDescription)]
  public bool Force { get; set; }

  public void Run() => RunAsync().Wait();

  public async Task RunAsync()
  {
    SanitizeInput();
    ValidateInput();
    var wordCount = WordCount.Combine(await GetWordCountsAsync());
    await provider.GetRequiredService<IFileWriter>().WriteOutputFilesAsync(OutputPath, wordCount);
  }

  private async Task<WordCount[]> GetWordCountsAsync()
  {
    var fileReader = provider.GetRequiredService<IFileReader>();
    var inputFileNames = Directory.GetFiles(InputPath).Where(fileName => !fileName.EndsWith("exclude.txt"));
    var excludedWords = await GetExcludedWordsAsync();
    var wordCountTasks = inputFileNames.Select(fileName => fileReader.GetWordCountAsync(fileName, excludedWords));
    return await Task.WhenAll(wordCountTasks);
  }

  private async Task<string[]> GetExcludedWordsAsync()
  {
    var excludeFilePath = Path.Combine(InputPath, "exclude.txt");
    var excludedWords = File.Exists(excludeFilePath)
      ? await File.ReadAllLinesAsync(excludeFilePath)
      : Array.Empty<string>();
    var validationResult = await provider.GetRequiredService<ExcludeFileValidator>().ValidateAsync(excludedWords);
    if (!validationResult.IsValid)
      Environment.Exit(11);
    return excludedWords;
  }

  private void SanitizeInput() => Extension = Extension.TrimStart('.');

  private void ValidateInput()
  {
    var validation = provider
      .GetRequiredService<CountWordsValidator>()
      .Validate(this);

    if (!validation.IsValid)
      Environment.Exit(11);
  }
}