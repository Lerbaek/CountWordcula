using CountWordcula.Backend;
using CountWordcula.Backend.FileWrite;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Test.FileWrite;

[Collection(nameof(FileWriter))]
public class FileWriterTests
{
  private readonly IFileWriter uut;

  public FileWriterTests(IFileWriter uut) => this.uut = uut;

  [Fact]
  public async Task WriteOutputFilesAsync_WriteKnownValues_ProvidedValuesAreWritten()
  {
    var wordCount = new WordCount
    {
      { "ALPHA",      1 },
      { "BETA",      22 },
      { "CHI",      333 },
      { "CHARLIE", 4444 },
      { "BRAVO",  55555 }
    };

    await uut.WriteOutputFilesAsync(
      ResultDirectoryName,
      wordCount);

    using (new AssertionScope())
    {
      foreach (var grouping in wordCount.Keys.GroupBy(k => k[0]))
      {
        var lines = await File.ReadAllLinesAsync(Path.Combine(ResultDirectoryName, OutputFileName(grouping.Key)));
        lines
          .Should()
          .BeEquivalentTo(
            grouping.Select(word => $"{word} {wordCount[word]}"),
            "values written to disk must match input values");
      }
    }
  }
}