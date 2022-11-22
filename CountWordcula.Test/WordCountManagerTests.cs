using CountWordcula.Backend;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Xunit;
using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Test
{
  [Collection(nameof(Backend.FileWrite.FileWriter))]
  public class WordCountManagerTests
  {
    private readonly IWordCountManager uut;
    private readonly ILogger<WordCountManagerTests> logger;

    public WordCountManagerTests(IWordCountManager uut, ILogger<WordCountManagerTests> logger)
    {
      this.uut = uut;
      this.logger = logger;

      var inputPath = Path.Combine(Environment.CurrentDirectory, SampleInputDirectoryName);
      var inputExtension = "txt";
      var outputPath = Path.Combine(Environment.CurrentDirectory, ResultDirectoryName);
      var force = true;

      configuration = new WordCountConfiguration(
        inputPath,
        inputExtension,
        outputPath,
        force);
    }

    [Fact]
    public async Task RunAsync_CountOutputFiles_OutputFilesExist()
    {
      (await GetOutputFiles())
        .Should()
        .NotBeEmpty($"{nameof(uut.RunAsync)}() method should have produced output files to {configuration.OutputPath}");
    }

    [SkippableTheory]
    [InlineData("A", true)]
    [InlineData("B", true)]
    [InlineData("C", true)]
    [InlineData("D", true)]
    [InlineData("E", true)]
    [InlineData("F", true)]
    [InlineData("G", true)]
    [InlineData("H", true)]
    [InlineData("I", true)]
    [InlineData("J", true)]
    [InlineData("K", false)]
    [InlineData("L", true)]
    [InlineData("M", true)]
    [InlineData("N", true)]
    [InlineData("O", true)]
    [InlineData("P", true)]
    [InlineData("Q", true)]
    [InlineData("R", true)]
    [InlineData("S", true)]
    [InlineData("T", true)]
    [InlineData("U", true)]
    [InlineData("V", true)]
    [InlineData("W", false)]
    [InlineData("X", false)]
    [InlineData("Y", false)]
    [InlineData("Z", false)]
    [InlineData("EXCLUDED", true)]
    public async Task RunAsync_CountOutputFiles_ExpectedFilesExistWithContent(string letter, bool expected)
    {
      if (!(await GetOutputFiles()).Any())
      {
        logger.LogWarning($"Skipping test because no output files could be found. Please refer to {nameof(RunAsync_CountOutputFiles_OutputFilesExist)}");
        throw new SkipException();
      }

      var fileName = OutputFileName(letter);
      var filePath = Path.Combine(configuration.OutputPath, fileName);
      var fileExists = File.Exists(filePath);

      using (new AssertionScope())
      {
        fileExists.Should().Be(expected);
        if (fileExists)
          (await File.ReadAllTextAsync(filePath))
            .Trim()
            .Should()
            .NotBeEmpty();
      }
    }

    [Fact]
    public async Task RunAsync_CountWords_WordCountMatchesActualCount()
    {
      var inputWords = await ReadInputWords();
      var excludedWords = await ReadExcludedWords();
      var outputFilePaths = await GetOutputFiles();

      var inputWordCount = ResolveInputWordCount(inputWords, excludedWords);
      var outputWordCount = ReadOutputWordCount(outputFilePaths);

      var inputExcludedWordsCount = inputWords.LongCount(word => excludedWords.Contains(word));
      var outputExcludedWordsCount = await ResolveOutputExcludedWordsCount(outputFilePaths);

      using (new AssertionScope())
      {
        inputWordCount.Should().BeEquivalentTo(outputWordCount);
        inputExcludedWordsCount.Should().Be(outputExcludedWordsCount);
      }
    }

    private static async Task<long> ResolveOutputExcludedWordsCount(string[] outputFilePaths)
    {
      var outputExcludedWordsFilePath = outputFilePaths.Single(IsExcludeFile);
      var outputExcludeFileText = await File.ReadAllTextAsync(outputExcludedWordsFilePath);
      var outputExcludedWordsCount = long.Parse(
        outputExcludeFileText.Split().Last());
      return outputExcludedWordsCount;
    }

    private static Dictionary<string, long> ReadOutputWordCount(string[] outputFilePaths)
    {
      var outputWordCount = outputFilePaths
        .Where(filePath => !IsExcludeFile(filePath))
        .SelectMany(
          filePath => File
            .ReadAllLines(filePath)
            .Select(line => line.Split()))
        .ToDictionary(line => line[0], line => long.Parse(line[1]));
      return outputWordCount;
    }

    private static Dictionary<string, long> ResolveInputWordCount(string[] sanitizedInputWords, string[] excludedWords)
    {
      var inputWordCount = sanitizedInputWords
        .GroupBy(word => word, words => words.Length)
        .Where(group => !excludedWords.Contains(group.Key))
        .ToDictionary(
          group => group.Key,
          group => group.LongCount());
      return inputWordCount;
    }

    private async Task<string[]> ReadExcludedWords()
    {
      var excludeFilePath = Path.Combine(configuration.InputPath, InputExcludeFileName);
      var excludedWords = await File.ReadAllLinesAsync(excludeFilePath);
      return excludedWords;
    }

    private async Task<string[]> ReadInputWords()
    {
      var inputFiles =
        Directory.GetFiles(configuration.InputPath)
          .Where(
            filePath => Path.GetFileName(filePath)
              .StartsWith("Sample"));

      var allInputText = await Task.WhenAll(inputFiles.Select(async filePath => await File.ReadAllTextAsync(filePath)));
      var allInputWords = string.Join(" ", allInputText);

      var sanitizedInputWords = allInputWords
        .Split()
        .Where(word => !string.IsNullOrWhiteSpace(word))
        .Select(
          word => word
            .TrimEnd('.', ',')
            .ToUpperInvariant())
        .ToArray();
      return sanitizedInputWords;
    }

    private static bool IsExcludeFile(string filePath) =>
      filePath.EndsWith(OutputExcludeFileName);

    private string[]? outputFiles;
    private readonly WordCountConfiguration configuration;

    private async Task<string[]> GetOutputFiles()
    {
        if (outputFiles != null)
          return outputFiles;

        if(Directory.Exists(configuration.OutputPath) && configuration.Force)
          Directory.Delete(configuration.OutputPath, true);

        if (!Directory.Exists(configuration.OutputPath))
          Directory.CreateDirectory(configuration.OutputPath);

        await uut.RunAsync(configuration);
        return outputFiles = Directory.GetFiles(configuration.OutputPath);
    }
  }
}