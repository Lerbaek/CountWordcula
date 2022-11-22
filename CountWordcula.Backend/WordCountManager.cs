﻿using CountWordcula.Backend.FileReader;
using CountWordcula.Backend.FileWriter;
using CountWordcula.Backend.Validate;
using static CountWordcula.Backend.Register.ConfiguratonRegister;

namespace CountWordcula.Backend;

public class WordCountManager : IWordCountManager
{
  private readonly IFileReader fileReader;
  private readonly IFileWriter fileWriter;
  private readonly WordCountConfigurationValidator wordCountConfigurationValidator;
  private readonly ExcludeFileValidator excludeFileValidator;

  public WordCountManager(
    IFileReader fileReader,
    IFileWriter fileWriter,
    WordCountConfigurationValidator wordCountConfigurationValidator,
    ExcludeFileValidator excludeFileValidator)
  {
    this.fileReader = fileReader;
    this.fileWriter = fileWriter;
    this.wordCountConfigurationValidator = wordCountConfigurationValidator;
    this.excludeFileValidator = excludeFileValidator;
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
    var excludedWords = await GetExcludedWordsAsync(configuration.InputPath);
    if (!await Validate(configuration, excludedWords))
      return false;
    await ExecuteAsync(configuration, excludedWords);
    return true;
  }

  private async Task<bool> Validate(WordCountConfiguration configuration, string[] excludedWords)
  {
    if (!await ValidateInput(configuration))
      return false;
    if (!await ValidateExcludedWords(excludedWords))
      return false;
    return true;
  }

  private async Task<bool> ValidateInput(WordCountConfiguration configuration)
  {
    var validationResult = await wordCountConfigurationValidator.ValidateAsync(configuration);
    return validationResult.IsValid;
  }

  private async Task<bool> ValidateExcludedWords(string[] words)
  {
    var validationResult = await excludeFileValidator.ValidateAsync(words);
    return validationResult.IsValid;
  }

  private async Task ExecuteAsync(
    WordCountConfiguration configuration,
    string[]? excludedWords = null)
  {
    var inputFileNames = Directory
      .GetFiles(configuration.InputPath)
      .Where(fileName => fileName.EndsWith(configuration.InputExtension) && !fileName.EndsWith(ExcludeFileName));

    excludedWords ??= await GetExcludedWordsAsync(configuration.InputPath);
    var wordCountTasks = inputFileNames.Select(fileName => fileReader.GetWordCountAsync(fileName, excludedWords));
    var wordCounts = await Task.WhenAll(wordCountTasks);

    var wordCount = WordCount.Combine(wordCounts);
    await fileWriter.WriteOutputFilesAsync(configuration.OutputPath, wordCount);
  }

  private static async Task<string[]> GetExcludedWordsAsync(string inputPath)
  {
    var excludeFilePath = Path.Combine(inputPath, ExcludeFileName);

    return File.Exists(excludeFilePath)
      ? await File.ReadAllLinesAsync(excludeFilePath)
      : Array.Empty<string>();
  }
}