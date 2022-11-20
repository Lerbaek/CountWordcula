using System.Collections.Concurrent;

namespace CountWordcula.Backend.FileReader;

public class MemoryEfficientParallelFileReader : IFileReader
{
  private readonly ConcurrentDictionary<string, long> wordCount;

  public MemoryEfficientParallelFileReader() => wordCount = new ConcurrentDictionary<string, long>();

  public async Task<IDictionary<string, long>> GetWordCountAsync(string fileName)
  {
    using var reader = File.OpenText(fileName);
    var word = string.Empty;
    var tasks = new List<Task>();
    while (reader.Peek() >= 0)
    {
      var line = await reader.ReadLineAsync();
      tasks.Add(Task.Run(
        () =>
        {
          foreach (var word in line!.Split()
                     .Select(w => w.TrimEnd(',', '.'))
                     .Where(w => !string.IsNullOrWhiteSpace(w)))
          {
            wordCount[word] = wordCount.ContainsKey(word)
              ? wordCount[word] + 1
              : 1;
          }
        }));
    }

    await Task.WhenAll(tasks);
    return wordCount;
  }
}