namespace CountWordcula.Backend.FileRead;

/// <summary>
/// Common interface for retrieving word counts.
/// </summary>
public interface IFileReader
{
  /// <summary>
  /// Read a file and return a filtered count of each word.
  /// </summary>
  /// <param name="fileName">Absolute or relative path to the file to read.</param>
  /// <param name="exclude">Words that should not be explicitly counted. A total count of excluded words will instead be found in <see cref="WordCount.Excluded"/>.</param>
  /// <returns>A count of all encountered words and a combined count of encountered excluded words.</returns>
  Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude);
}