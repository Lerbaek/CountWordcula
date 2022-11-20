namespace CountWordcula.Backend.FileReader;

public class FluentFileReader : IFileReader
{
  public async Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude)
  {
    var allText = await File.ReadAllTextAsync(fileName);
    var excludedCount = 0;
    var wordCountAsync = allText.Split()
      .Select(word => word.TrimEnd(',', '.').ToUpperInvariant())
      .Where(word => !string.IsNullOrWhiteSpace(word))
      .Where(word =>
      {
        var excluded = exclude.Contains(word, StringComparer.InvariantCultureIgnoreCase);
        if (excluded)
          excludedCount++;
        return !excluded;
      })
      .GroupBy(word => word[0])
      .ToDictionary(group => group.Key, group => group.LongCount());
    return new WordCount(wordCountAsync, excludedCount);
  }
}