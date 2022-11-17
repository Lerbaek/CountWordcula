using CountWordcula.Validate;
using GoCommando;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CountWordcula.Count;

[Command("count-words")]
[Description("Read data from multiple files in a given directory.")]
public class CountWords : ICountWords
{
  #region Names

  private const string InputPathName = "input-path";
  private const string InputPathShortName = "i";

  private const string OutputPathName = "output-path";
  private const string OutputPathShortName = "o";

  private const string ExtensionName = "extension";
  private const string ExtensionShortName = "e";

  private const string SegregateName = "segregate";
  private const string SegregateShortName = "s";

  private const string ForceName = "force";
  private const string ForceShortName = "f";

  #endregion

  #region Descriptions

  private const string InputPathDescription  = "Where are the input files?";
  private const string OutputPathDescription = "Where should the output files be placed?";
  private const string ExtensionDescription  = "Which file extension does the input file(s) have?";
  private const string SegregateDescription  = "Should there be separate output files per input file?";
  private const string ForceDescription      = "Should any existing files be overwritten?";

  #endregion

  #region Optional

  private const bool ExtensionOptional = true;
  private const bool SegregateOptional = true;
  private const bool ForceOptional     = true;

  #endregion

  #region Default values

  private const string ExtensionDefaultValue = "txt";

  #endregion

  private static readonly IServiceProvider Provider;

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

  [Parameter(InputPathName, InputPathShortName)]
  [Description(InputPathDescription)]
  public string InputPath { get; set; } = null!;

  [Parameter(OutputPathName, OutputPathShortName)]
  [Description(OutputPathDescription)]
  public string OutputPath { get; set; } = null!;

  [Parameter(ExtensionName, ExtensionShortName, ExtensionOptional, ExtensionDefaultValue)]
  [Description(ExtensionDescription)]
  public string Extension { get; set; } = ExtensionDefaultValue;


  [Parameter(SegregateName, SegregateShortName, SegregateOptional)]
  [Description(SegregateDescription)]
  public bool Segregate { get; set; }

  [Parameter(ForceName, ForceShortName, ForceOptional)]
  [Description(ForceDescription)]
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