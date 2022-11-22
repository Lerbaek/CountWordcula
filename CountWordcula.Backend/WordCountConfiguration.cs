namespace CountWordcula.Backend;

public class WordCountConfiguration
{
  public string? InputPath { get; set; }
  public string? InputExtension { get; set; }
  public string? OutputPath { get; set; }
  public bool Force { get; set; }

  public WordCountConfiguration(
    string? inputPath,
    string? inputExtension,
    string? outputPath,
    bool force = false)
  {
    InputPath = inputPath;
    InputExtension = inputExtension?.TrimStart('.');
    OutputPath = outputPath;
    Force = force;
  }
}