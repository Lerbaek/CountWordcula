﻿using CountWordcula.Count;
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
    public void Run_WordsAreCounted_OutputFilesContainCorrectData()
    {
      if (Directory.Exists(uut.OutputPath))
        Directory.CreateDirectory(uut.OutputPath);

      uut.Run();
    }
  }
}