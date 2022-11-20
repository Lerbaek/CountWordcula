using CountWordcula.Backend.FileReader;
using CountWordcula.Test.TestData;
using FluentAssertions;
using Xunit;

namespace CountWordcula.Test.FileReader;

public abstract class FileReaderTestsBase
{
  private readonly IFileReader uut;
  public FileReaderTestsBase(IFileReader uut) => this.uut = uut;

  [Theory]
  [ClassData(typeof(SamplePathProvider))]
  public async Task GetWordCountAsync_FileExists_WordCountReturned(string path)
  {
    var expectedWordCount = long.Parse(Path.GetFileNameWithoutExtension(path)
      .Substring("Sample_".Length));
    var wordCountAsync = await uut.GetWordCountAsync(path);
    wordCountAsync.Values.Sum()
      .Should()
      .Be(expectedWordCount, $"that is the amount of words in the sample file {Path.GetFileName(path)}");
    wordCountAsync.Should()
      .NotBeEmpty();
  }
}