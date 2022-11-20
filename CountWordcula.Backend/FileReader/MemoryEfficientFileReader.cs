using System.Collections.Concurrent;

namespace CountWordcula.Backend.FileReader;

public class MemoryEfficientFileReader : IFileReader
{
  private Dictionary<char, long> wordCount = null!;

  public Task<IDictionary<char, long>> GetWordCountAsync(string fileName)
  {
    using var reader = File.OpenText(fileName);
    var word = string.Empty;
    wordCount = new Dictionary<char, long>();
    while (reader.Peek() >= 0)
    {
      var character = (char)reader.Read();
      if (character is '.' or ',')
        continue;
      if (character is ' ' or '\r' or '\n' && !string.IsNullOrWhiteSpace(word))
      {
        CountWord(word);
        word = string.Empty;
      }
      else
        word += character;
    }
    if(!string.IsNullOrWhiteSpace(word))
      CountWord(word);

    return Task.FromResult<IDictionary<char, long>>(wordCount);
  }

  private void CountWord(string word)
  {
    var firstLetter = char.ToUpperInvariant(word[0]);
    wordCount[firstLetter] = wordCount.ContainsKey(firstLetter)
      ? wordCount[firstLetter] + 1
      : 1;
  }
}