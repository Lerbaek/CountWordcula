namespace CountWordcula.Backend;

/// <summary>
/// Input value data class
/// </summary>
public class WordCountConfiguration
{
  /// <summary>
  /// The path to a directory containing one or more input files.
  /// </summary>
  public string? InputPath { get; set; }

  /// <summary>
  /// The extension of the files from which to count words.
  /// </summary>
  public string? InputExtension { get; set; }

  /// <summary>
  /// The path to a directory in which the output files will be placed.
  /// </summary>
  public string? OutputPath { get; set; }

  /// <summary>
  /// Whether any existing files should be overwritten.
  /// </summary>
  public bool Force { get; set; }

  /// <inheritdoc cref="WordCountConfiguration"/>
  /// <param name="inputPath">The path to a directory containing one or more input files.</param>
  /// <param name="inputExtension">The extension of the files from which to count words.</param>
  /// <param name="outputPath">The path to a directory in which the output files will be placed.</param>
  /// <param name="force">Whether any existing files should be overwritten.</param>
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