namespace CountWordcula.Backend.FileRead;

/// <summary>
/// A file reader with focus on low memory usage rather than performance.
/// </summary>
public class MemoryEfficientFileReader : IFileReader
{
  /// <inheritdoc />
  public Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude) =>
    Task.Run(
      () =>
      {
        using var reader = File.OpenText(fileName);
        var word = string.Empty;
        var wordCount = new WordCount();
        while (reader.Peek() >= 0)
        {
          var character = (char)reader.Read();
          if (character is '.' or ',')
            continue;
          if (character is ' ' or '\r' or '\n')
          {
            if (string.IsNullOrWhiteSpace(word))
              continue;

            CountWord(
              word,
              wordCount,
              exclude);
            word = string.Empty;
          }
          else
            word += char.ToUpperInvariant(character);
        }

        if (!string.IsNullOrWhiteSpace(word))
          CountWord(
            word,
            wordCount,
            exclude);
        return wordCount;
      });

  private void CountWord(string word, WordCount wordCount, string[] exclude)
  {
    if (exclude.Contains(word, StringComparer.InvariantCultureIgnoreCase))
    {
      wordCount.Excluded++;
      return;
    }
    wordCount[word] = wordCount.ContainsKey(word)
      ? wordCount[word] + 1
      : 1;
  }
}