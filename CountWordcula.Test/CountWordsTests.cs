using CountWordcula.Command;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Xunit;
using static CountWordcula.Backend.Register.ConfiguratonRegister;

namespace CountWordcula.Test
{
  public class CountWordsTests
  {
    private readonly CountWordsCommand uut;
    private readonly ILogger<CountWordsTests> logger;

    public CountWordsTests(CountWordsCommand uut, ILogger<CountWordsTests> logger)
    {
      this.uut = uut;
      this.logger = logger;

      uut.OutputPath = Path.Combine(Environment.CurrentDirectory, "Results");
      uut.InputPath = Path.Combine(Environment.CurrentDirectory, "Samples");
      uut.Force = true;
    }

    [Fact]
    public async Task Run_CountOutputFiles_OutputFilesExist()
    {
      (await GetOutputFiles())
        .Should()
        .NotBeEmpty($"{nameof(uut.Run)}() method should have produced output files to {uut.OutputPath}");
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
    public async Task Run_CountOutputFiles_ExpectedFilesExistWithContent(string letter, bool expected)
    {
      if (!(await GetOutputFiles()).Any())
      {
        logger.LogWarning($"Skipping test because no output files could be found. Please refer to {nameof(Run_CountOutputFiles_OutputFilesExist)}");
        throw new SkipException();
      }

      var fileName = $"FILE_{letter}.txt";
      var filePath = Path.Combine(uut.OutputPath, fileName);
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
    public async Task Run_CountWords_WordCountMatchesActualCount()
    {
      var inputFiles =
        Directory.GetFiles(uut.InputPath)
          .Where(filePath => Path.GetFileName(filePath).StartsWith("Sample"));

      var allInputText = await Task.WhenAll(inputFiles.Select(async filePath => await File.ReadAllTextAsync(filePath)));
      var allInputWords = string.Join(" ", allInputText);

      var sanitizedInputWords = allInputWords
        .Split()
        .Where(word => !string.IsNullOrWhiteSpace(word))
        .Select(word => word
          .TrimEnd('.', ',')
          .ToUpperInvariant())
        .ToArray();

      var excludeFilePath = Path.Combine(uut.InputPath, ExcludeFileName);
      var excludedWords = await File.ReadAllLinesAsync(excludeFilePath);

      var inputWordCount = sanitizedInputWords
        .GroupBy(word => word, words => words.Length)
        .Where(group => !excludedWords.Contains(group.Key))
        .ToDictionary(
          group => group.Key,
          group => group.LongCount());

      var outputFilePaths = await GetOutputFiles();

      var outputWordCount = outputFilePaths
        .Where(filePath => !IsExcludeFile(filePath))
        .SelectMany(filePath => File
          .ReadAllLines(filePath)
          .Select(line => line.Split()))
        .ToDictionary(line => line[0], line => long.Parse(line[1]));

      var inputExcludedWordsCount = sanitizedInputWords.LongCount(word => excludedWords.Contains(word));
      var outputExcludedWordsFilePath = outputFilePaths.Single(IsExcludeFile);
      var outputExcludeFileText = await File.ReadAllTextAsync(outputExcludedWordsFilePath);
      var outputExcludedWordsCount = long.Parse(outputExcludeFileText.Split().Last());

      using (new AssertionScope())
      {
        inputWordCount.Should()
          .BeEquivalentTo(outputWordCount);

        inputExcludedWordsCount.Should().Be(outputExcludedWordsCount);
      }
    }

    private static bool IsExcludeFile(string filePath) =>
      Path.GetFileName(filePath).StartsWith("FILE_EXCLUDE");

    private string[]? outputFiles;
    private async Task<string[]> GetOutputFiles()
    {
        if (outputFiles != null)
          return outputFiles;

        if(Directory.Exists(uut.OutputPath) && uut.Force)
          Directory.Delete(uut.OutputPath, true);

        if (!Directory.Exists(uut.OutputPath))
          Directory.CreateDirectory(uut.OutputPath);

        await uut.RunAsync();
        return outputFiles = Directory.GetFiles(uut.OutputPath);
    }
  }
}