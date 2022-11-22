namespace CountWordcula.Backend.Register;

public class ConfigurationRegister
{
  public const string InputExcludeFileName = "exclude.txt";
  public const string SampleInputDirectoryName = "Samples";
  public const string ResultDirectoryName = "Results";

  public static readonly string OutputExcludeFileName = OutputFileName("EXCLUDED");

  public static string OutputFileName(string key) => $"FILE_{key}.txt";
  public static string OutputFileName(char key) => OutputFileName(key.ToString());
}