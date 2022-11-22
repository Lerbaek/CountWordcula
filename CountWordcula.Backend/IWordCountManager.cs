namespace CountWordcula.Backend;

/// <summary>
/// Orchestrator class for counting words.
/// </summary>
public interface IWordCountManager
{
  /// <summary>
  /// Perform a word count based on an input configuration.
  /// </summary>
  /// <returns>Whether the methods has encountered validation or logical errors.</returns>
  Task<bool> RunAsync(WordCountConfiguration configuration);

  /// <inheritdoc cref="RunAsync(WordCountConfiguration)"/>
  /// <inheritdoc cref="WordCountConfiguration(string,string,string,bool)"/>
  Task<bool> RunAsync(string inputPath, string inputExtension, string outputPath, bool force = false);
}