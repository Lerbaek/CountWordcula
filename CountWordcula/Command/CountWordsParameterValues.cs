namespace CountWordcula.Command;

public class CountWordsParameterValues
{
  #region Names

  public const string InputPathName = "input-path";
  public const string InputPathShortName = "i";

  public const string OutputPathName = "output-path";
  public const string OutputPathShortName = "o";

  public const string ExtensionName = "extension";
  public const string ExtensionShortName = "e";

  public const string SegregateName = "segregate";
  public const string SegregateShortName = "s";

  public const string ForceName = "force";
  public const string ForceShortName = "f";

  #endregion

  #region Descriptions

  public const string InputPathDescription  = "Where are the input files?";
  public const string OutputPathDescription = "Where should the output files be placed?";
  public const string ExtensionDescription  = "Which file extension does the input file(s) have?";
  public const string SegregateDescription  = "Should there be separate output files per input file?";
  public const string ForceDescription      = "Should any existing files be overwritten?";

  #endregion

  #region Optional

  public const bool ExtensionOptional = true;
  public const bool SegregateOptional = true;
  public const bool ForceOptional     = true;

  #endregion

  #region Default values

  public const string ExtensionDefaultValue = "txt";

  #endregion
}