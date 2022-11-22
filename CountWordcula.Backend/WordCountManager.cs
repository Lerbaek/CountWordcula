using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.FileWrite;
using CountWordcula.Backend.Registry;
using CountWordcula.Backend.Validate;
using Microsoft.Extensions.Logging;
using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Backend;

/// <inheritdoc cref="IWordCountManager"/>
public class WordCountManager : IWordCountManager
{
  private readonly IFileReader fileReader;
  private readonly IFileWriter fileWriter;
  private readonly WordCountConfigurationValidator wordCountConfigurationValidator;
  private readonly ExcludeFileValidator excludeFileValidator;
  private readonly ILogger<WordCountManager> logger;

  /// <inheritdoc cref="WordCountManager"/>
  public WordCountManager(
    IFileReader fileReader,
    IFileWriter fileWriter,
    WordCountConfigurationValidator wordCountConfigurationValidator,
    ExcludeFileValidator excludeFileValidator,
    ILogger<WordCountManager> logger)
  {
    this.fileReader = fileReader;
    this.fileWriter = fileWriter;
    this.wordCountConfigurationValidator = wordCountConfigurationValidator;
    this.excludeFileValidator = excludeFileValidator;
    this.logger = logger;
  }

  public async Task<bool> RunAsync(string inputPath, string inputExtension, string outputPath, bool force = false) =>
    await RunAsync(
      new WordCountConfiguration(
        inputPath,
        inputExtension,
        outputPath,
        force));

  public async Task<bool> RunAsync(WordCountConfiguration configuration)
  {
    logger.LogInformation("Reading excluded words.");
    var excludedWords = await GetExcludedWordsAsync(configuration.InputPath!);
    logger.LogDebug("{Count} lines of excluded words found.", excludedWords.Length);
    if (!await Validate(configuration, excludedWords))
      return false;
    await ExecuteAsync(configuration, excludedWords);
    return true;
  }

  private async Task<bool> Validate(WordCountConfiguration configuration, string[] excludedWords)
  {
    logger.LogInformation("Validating input configuration.");
    var inputValid = await ValidateInput(configuration);
    logger.LogDebug("Input configuration is {Validity}.", (inputValid ? "" : "not ") + "valid");

    logger.LogInformation("Validating excluded words.");
    var excludedWordsValid = await ValidateExcludedWords(excludedWords);
    logger.LogDebug("Excluded words are {Validity}.", (excludedWordsValid ? "" : "not ") + "valid");

    return inputValid && excludedWordsValid;
  }

  private async Task<bool> ValidateInput(WordCountConfiguration configuration) =>
    (await wordCountConfigurationValidator.ValidateAsync(configuration)).IsValid;

  private async Task<bool> ValidateExcludedWords(string[] words) =>
    (await excludeFileValidator.ValidateAsync(words)).IsValid;

  private async Task ExecuteAsync(
    WordCountConfiguration configuration,
    string[]? excludedWords = null)
  {
    logger.LogInformation(
      "Reading input files with extension {Extension} from {InputPath}",
      $".{configuration.InputExtension}",
      configuration.InputPath);

    var inputFileNames = Directory
      .GetFiles(configuration.InputPath!)
      .Where(fileName => fileName.EndsWith(configuration.InputExtension!) && !fileName.EndsWith(InputExcludeFileName))
      .ToArray();

    logger.LogInformation("{Count} input files found.", inputFileNames.Length);
    foreach (var fileName in inputFileNames.Select(Path.GetFileName))
      logger.LogDebug("{FileName}", fileName);

    if (excludedWords is null)
    {
      logger.LogInformation(
        "No excluded words were provided. Reading from {ExcludedFileName} instead.",
        InputExcludeFileName);

      excludedWords = await GetExcludedWordsAsync(configuration.InputPath!);
    }

    logger.LogInformation("Counting words ...");
    var wordCountTasks = inputFileNames.Select(fileName => fileReader.GetWordCountAsync(fileName, excludedWords));
    var wordCounts = await Task.WhenAll(wordCountTasks);

    logger.LogInformation("Done!");

    var wordCount = WordCount.Combine(wordCounts);

    logger.LogInformation("Writing word counts to per-letter files in {OutputPath}", configuration.OutputPath);
    await fileWriter.WriteOutputFilesAsync(configuration.OutputPath!, wordCount);
    logger.LogInformation("All done. See output files in {AbsoluteOutputPath}", Path.GetFullPath(configuration.OutputPath!));
  }

  private static async Task<string[]> GetExcludedWordsAsync(string inputPath)
  {
    var excludeFilePath = Path.Combine(inputPath, InputExcludeFileName);

    return File.Exists(excludeFilePath)
      ? await File.ReadAllLinesAsync(excludeFilePath)
      : Array.Empty<string>();
  }
}