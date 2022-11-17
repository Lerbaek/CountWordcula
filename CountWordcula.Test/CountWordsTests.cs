using CountWordcula.Count;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CountWordcula.Test
{
  public class CountWordsTests
  {
    private readonly ICountWords uut;
    private readonly ILogger<CountWordsTests> logger;

    public CountWordsTests(ICountWords uut, ILogger<CountWordsTests> logger)
    {
      this.uut = uut;
      this.logger = logger;

      uut.OutputPath = Path.Combine(Environment.CurrentDirectory, "Results");
      uut.InputPath = Path.Combine(Environment.CurrentDirectory, "Samples");
      uut.Force = true;
    }

    [Fact]
    public void Run_CountOutputFiles_OutputFilesExist()
    {
      OutputFiles
        .Should()
        .NotBeEmpty($"{nameof(uut.Run)}() method should have produced output files to {uut.OutputPath}");
    }

    [SkippableTheory]
    [InlineData('A', true)]
    [InlineData('B', true)]
    [InlineData('C', true)]
    [InlineData('D', true)]
    [InlineData('E', true)]
    [InlineData('F', true)]
    [InlineData('G', true)]
    [InlineData('H', true)]
    [InlineData('I', true)]
    [InlineData('J', true)]
    [InlineData('K', false)]
    [InlineData('L', true)]
    [InlineData('M', true)]
    [InlineData('N', true)]
    [InlineData('O', true)]
    [InlineData('P', true)]
    [InlineData('Q', true)]
    [InlineData('R', true)]
    [InlineData('S', true)]
    [InlineData('T', true)]
    [InlineData('U', true)]
    [InlineData('V', true)]
    [InlineData('W', false)]
    [InlineData('X', false)]
    [InlineData('Y', false)]
    [InlineData('Z', false)]
    public void Run_CountOutputFiles_ExpectedFilesExistWithContent(char letter, bool expected)
    {
      if (!OutputFiles.Any())
      {
        logger.LogWarning($"Skipping test because no output files could be found. Please refer to {nameof(Run_CountOutputFiles_OutputFilesExist)}");
        throw new SkipException();
      }

      var fileName = $"FILE_{letter}.{CountWords.ExtensionDefaultValue}";
      var filePath = Path.Combine(uut.OutputPath, fileName);
      var fileExists = File.Exists(filePath);

      using (new AssertionScope())
      {
        fileExists.Should().Be(expected);
        if (fileExists)
          File.ReadAllText(filePath)
            .Trim()
            .Should()
            .NotBeEmpty();
      }
    }

    private string[]? outputFiles;
    private string[] OutputFiles
    {
      get
      {
        if (outputFiles != null)
          return outputFiles;

        if(Directory.Exists(uut.OutputPath) && uut.Force)
          Directory.Delete(uut.OutputPath, true);

        if (!Directory.Exists(uut.OutputPath))
          Directory.CreateDirectory(uut.OutputPath);

        uut.Run();
        return outputFiles = Directory.GetFiles(uut.OutputPath);
      }
    }
  }
}