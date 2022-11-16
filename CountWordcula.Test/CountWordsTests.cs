using CountWordcula.Read;
using Xunit;

namespace CountWordcula.Test
{
  public class CountWordsTests
  {
    private readonly ICountWords uut;

    public CountWordsTests(ICountWords uut) => this.uut = uut;

    [Fact]
    public void Run_WordsAreCounted_OutputFilesContainCorrectData()
    {
    }
  }
}