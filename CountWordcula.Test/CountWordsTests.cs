using CountWordcula.Count;
using FluentAssertions;
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
    [InlineData("a", true)]
    public void Run_CountOutputFiles_ExpectedFilesExist(string letter, bool expected)
    {
      if (!OutputFiles.Any())
      {
        logger.LogWarning($"Skipping test because no output files could be found. Please refer to {nameof(Run_CountOutputFiles_OutputFilesExist)}");
        throw new SkipException();
      }

      var fileName = $"FILE_{letter.ToUpper()}.{CountWords.ExtensionDefaultValue}";
      var filePath = Path.Combine(uut.OutputPath, fileName);
      File.Exists(filePath).Should().Be(expected);
    }

    private string[] OutputFiles
    {
      get
      {
        if(Directory.Exists(uut.OutputPath) && uut.Force)
          Directory.Delete(uut.OutputPath, true);

        if (!Directory.Exists(uut.OutputPath))
          Directory.CreateDirectory(uut.OutputPath);

        uut.Run();
        return Directory.GetFiles(uut.OutputPath);
      }
    }
  }
}