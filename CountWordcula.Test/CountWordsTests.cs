using Castle.Core.Logging;
using CountWordcula.Count;
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
    }

    [Fact]
    public void Run_WordsAreCounted_OutputFilesContainCorrectData()
    {
      logger.LogInformation("Logging works.");
    }
  }
}