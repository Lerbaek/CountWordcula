using CountWordcula.Backend;
using GoCommando;
using static CountWordcula.Command.CountWordsParameterValues;

namespace CountWordcula.Command;

/// <summary>
/// Console command for the <see cref="CountWordcula"/> application.
/// </summary>
[Command(CountWordsCommandName)]
[Description(CountWordsDescription)]
public class CountWordsCommand : ICommand
{
  private readonly IWordCountManager wordCountManager;

  /// <inheritdoc cref="CountWordsCommand"/>
  public CountWordsCommand(IWordCountManager wordCountManager) => this.wordCountManager = wordCountManager;

  [Parameter(InputPathName, InputPathShortName)]
  [Description(InputPathDescription)]
  public string InputPath { get; set; } = null!;

  [Parameter(OutputPathName, OutputPathShortName)]
  [Description(OutputPathDescription)]
  public string OutputPath { get; set; } = null!;

  [Parameter(ExtensionName, ExtensionShortName, ExtensionOptional, ExtensionDefaultValue)]
  [Description(ExtensionDescription)]
  public string Extension { get; set; } = ExtensionDefaultValue;

  [Parameter(ForceName, ForceShortName, ForceOptional)]
  [Description(ForceDescription)]
  public bool Force { get; set; }

  public void Run() => RunAsync().Wait();

  public async Task RunAsync()
  {
    var inputValid = await wordCountManager.RunAsync(
      InputPath,
      Extension,
      OutputPath,
      Force);
    if(!inputValid)
      Environment.Exit(11);
  }
}