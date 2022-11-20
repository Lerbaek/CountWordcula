using System.Collections.Concurrent;

namespace CountWordcula.Backend.FileReader;

public class MemoryEfficientFileReader : IFileReader
{
  private readonly ConcurrentDictionary<string, long> wordCount;

  public MemoryEfficientFileReader() => wordCount = new ConcurrentDictionary<string, long>();

  public Task<IDictionary<string, long>> GetWordCountAsync(string fileName)
  {
    using var reader = File.OpenText(fileName);
    var word = string.Empty;
    while (reader.Peek() >= 0)
    {
      var character = (char)reader.Read();
      if (character is '.' or ',')
        continue;
      if (character == ' ')
      {
        wordCount[word] = wordCount.ContainsKey(word) ? wordCount[word] + 1 : 1;
        word = string.Empty;
      }
      else
        word += character;
    }

    return Task.FromResult<IDictionary<string, long>>(wordCount);
  }
}