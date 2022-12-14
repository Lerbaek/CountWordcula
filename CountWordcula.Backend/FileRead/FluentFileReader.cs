namespace CountWordcula.Backend.FileRead;

/// <summary>
/// A file reader with a focus on a simple, fluent implementation which is easy to follow,
/// but not very optimized.
/// </summary>
public class FluentFileReader : IFileReader
{
  /// <inheritdoc />
  public async Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude)
  {
    var allText = await File.ReadAllTextAsync(fileName);
    var excludedCount = 0;
    var wordCount = allText.Split()
      .Select(word => word.TrimEnd(',', '.').ToUpperInvariant())
      .Where(word => !string.IsNullOrWhiteSpace(word))
      .Where(word =>
      {
        var excluded = exclude.Contains(word, StringComparer.InvariantCultureIgnoreCase);
        if (excluded)
          excludedCount++;
        return !excluded;
      })
      .GroupBy(word => word.ToUpperInvariant())
      .ToDictionary(group => group.Key, group => group.LongCount());
    return new WordCount(wordCount, excludedCount);
  }
}