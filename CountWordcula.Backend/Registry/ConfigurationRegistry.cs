#pragma warning disable CS1591
namespace CountWordcula.Backend.Registry;

/// <summary>
/// Registry for commonly used static or calculated values.
/// </summary>
public static class ConfigurationRegistry
{
  public const string InputExcludeFileName = "exclude.txt";
  public const string SampleInputDirectoryName = "Samples";
  public const string ResultDirectoryName = "Results";

  public static readonly string OutputExcludeFileName = OutputFileName("EXCLUDED");

  public static string OutputFileName(string key) => $"FILE_{key}.txt";
  public static string OutputFileName(char key) => OutputFileName(key.ToString());
}