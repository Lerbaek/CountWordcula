using CountWordcula.Backend.FileReader;
using CountWordcula.Test.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileReader;

public abstract class FileReaderTestsBase
{
  private readonly IFileReader uut;
  private readonly ITestOutputHelper output;

  protected FileReaderTestsBase(IFileReader uut, ITestOutputHelper output)
  {
    this.uut = uut;
    this.output = output;
  }

  [Theory]
  [ClassData(typeof(SamplePathProvider))]
  public async Task GetWordCountAsync_FileExists_WordCountAddsUp(string path)
  {
    var expectedWordCount = GetExpectedWordCount(path);
    output.WriteLine("Expected word count: {0}", expectedWordCount);
    var wordCount = await uut.GetWordCountAsync(path);
    var wordCountSum = wordCount.Values.Sum();
    output.WriteLine("Actual word count: {0}", wordCountSum);
    wordCountSum
      .Should()
      .Be(expectedWordCount, $"that is the amount of words in the sample file {Path.GetFileName(path)}");
  }
  
  [Theory]
  [ClassData(typeof(SamplePathProvider))]
  public async Task GetWordCountAsync_WordsExcluded_WordCountAndExcludedCountAddsUp(string path)
  {
    var expectedWordCount = GetExpectedWordCount(path);
    output.WriteLine("Expected total word count: {0}", expectedWordCount);
    string[] excludedWords = { "lorem", "ipsum" };
    output.WriteLine("Number of excluded words: {0}", excludedWords.Length);
    var wordCount = await uut.GetWordCountAsync(path, excludedWords);
    var wordCountSum = wordCount.Values.Sum();
    output.WriteLine("Actual included word count: {0}", wordCountSum);
    output.WriteLine("Actual excluded word count: {0}", wordCount.Excluded);
    using (new AssertionScope())
    {
      (wordCountSum + wordCount.Excluded)
        .Should()
        .Be(expectedWordCount, $"that is the amount of words in the sample file {Path.GetFileName(path)}");
      wordCountSum.Should().NotBe(0);
      wordCount.Excluded.Should().NotBe(0);
    }
  }

  private static long GetExpectedWordCount(string path)
  {
    var expectedWordCount = long.Parse(
      Path.GetFileNameWithoutExtension(path)
        .Substring("Sample_".Length));
    return expectedWordCount;
  }
}