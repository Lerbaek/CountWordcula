using System.Collections.Concurrent;

namespace CountWordcula.Backend.FileReader;

public class MemoryEfficientParallelFileReader : IFileReader
{
  public async Task<IDictionary<char, long>> GetWordCountAsync(string fileName)
  {
    using var reader = File.OpenText(fileName);
    var tasks = new List<Task<Dictionary<char, long>>>();
    while (reader.Peek() >= 0)
    {
      var line = await reader.ReadLineAsync();
      tasks.Add(Task.Run(
        () =>
        {
          var wordCount = new Dictionary<char, long>();
          foreach (var word in line!.Split()
                     .Select(w => w.TrimEnd(',', '.'))
                     .Where(w => !string.IsNullOrWhiteSpace(w)))
          {
            var firstLetter = char.ToUpperInvariant(word[0]);
            wordCount[firstLetter] = wordCount.ContainsKey(firstLetter)
              ? wordCount[firstLetter] + 1
              : 1;
          }
          return wordCount;
        }));
    }

    var wordCounts = await Task.WhenAll(tasks);

    //return wordCounts.SelectMany(wc => wc.Select(kvp => kvp))
    //  .GroupBy(kvp => kvp.Key, kvp => kvp.Value)
    //  .ToDictionary(
    //    g => g.Key,
    //    g => g.Sum());

    var wordCount = wordCounts[0];
    foreach (var dictionary in wordCounts.Skip(1))
    foreach (var kvp in dictionary)
      wordCount[kvp.Key] = wordCount.ContainsKey(kvp.Key)
        ? wordCount[kvp.Key] + kvp.Value
        : kvp.Value;

    return wordCount;
  }
}