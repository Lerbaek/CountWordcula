using CountWordcula.Validate;
﻿using GoCommando;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CountWordcula.Count;

[Command("count-words")]
[Description("Read data from multiple files in a given directory.")]
public class CountWords : ICountWords
{
  [Parameter("input-path", "i")]
  [Description("Where are the input files?")]
  static CountWords()
  {
    Provider = new ServiceCollection()
      .AddLogging(
        builder => builder.AddSerilog(
          new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger()))
      .AddSingleton<CountWordsValidator>()
      .BuildServiceProvider();
  }
  public string InputPath { get; set; } = null!;

  [Parameter("output-path", "o")]
  [Description("Where should the output files be placed?")]
  public string OutputPath { get; set; } = null!;

  [Parameter("extension", "e", true, "txt")]
  [Description("Where should the output files be placed?")]
  public string Extension { get; set; } = null!;


  [Parameter("segregate", "s", true)]
  [Description("Should there be separate output files per input file?")]
  public bool Segregate { get; set; }

  [Parameter("force", "f", true)]
  [Description("Should any existing files be overwritten?")]
  public bool Force { get; set; }

  public void Run()
  {
    ValidateInput();
  }

  private void ValidateInput()
  {
    var validation = Provider
      .GetRequiredService<CountWordsValidator>()
      .Validate(this);

    if (!validation.IsValid)
      Environment.Exit(1);
  }
}