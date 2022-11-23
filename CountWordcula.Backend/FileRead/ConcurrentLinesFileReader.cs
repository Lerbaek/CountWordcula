namespace CountWordcula.Backend.FileRead;

/// <summary>
/// A file reader with focus on CPU utilization, where each line in a file
/// is handled by a different thread.
/// Will perform better with more than one or a few lines.
/// </summary>
public class ConcurrentLinesFileReader : IFileReader
{
  /// <inheritdoc />
  public async Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude)
  {
    using var reader = File.OpenText(fileName);
    var tasks = new List<Task<WordCount>>();
    while (reader.Peek() >= 0)
    {
      var line = await reader.ReadLineAsync();
      tasks.Add(Task.Run(
        () =>
        {
          var wordCount = new WordCount();
          foreach (var word in line!.Split()
                     .Select(w => w.TrimEnd(',', '.').ToUpperInvariant())
                     .Where(w =>
                     {
                       if (string.IsNullOrWhiteSpace(w))
                         return false;
                       var excluded = exclude.Contains(w, StringComparer.InvariantCultureIgnoreCase);
                       if (excluded)
                         wordCount.Excluded++;
                       return !excluded;
                     }))
          {
            wordCount[word] = wordCount.ContainsKey(word)
              ? wordCount[word] + 1
              : 1;
          }
          return wordCount;
        }));
    }

    var wordCounts = await Task.WhenAll(tasks);

    return WordCount.Combine(wordCounts);
  }
}