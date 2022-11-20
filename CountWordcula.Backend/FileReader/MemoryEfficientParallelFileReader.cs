using System.Collections.Concurrent;

namespace CountWordcula.Backend.FileReader;

public class MemoryEfficientParallelFileReader : IFileReader
{
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
                     .Where(w => !string.IsNullOrWhiteSpace(w))
                     .Where(w =>
                     {
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

    //var wordCount = wordCounts.SelectMany(wc => wc.Select(kvp => kvp))
    //  .GroupBy(kvp => kvp.Key, kvp => kvp.Value)
    //  .ToDictionary(
    //    g => g.Key,
    //    g => g.Sum());
    //return new WordCount(wordCount, wordCounts.Sum(wc => wc.Excluded));

    var wordCount = wordCounts[0];
    foreach (var wc in wordCounts.Skip(1))
    {
      wordCount.Excluded += wc.Excluded;
      foreach (var kvp in wc)
        wordCount[kvp.Key] = wordCount.ContainsKey(kvp.Key)
          ? wordCount[kvp.Key] + kvp.Value
          : kvp.Value;
    }

    return wordCount;
  }
}