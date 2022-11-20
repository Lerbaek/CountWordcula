namespace CountWordcula.Backend.FileReader;

public class MemoryEfficientFileReader : IFileReader
{
  public Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude)
  {
    using var reader = File.OpenText(fileName);
    var word = string.Empty;
    var wordCount = new WordCount();
    while (reader.Peek() >= 0)
    {
      var character = (char)reader.Read();
      if (character is '.' or ',')
        continue;
      if (character is ' ' or '\r' or '\n' && !string.IsNullOrWhiteSpace(word))
      {
        CountWord(word, wordCount, exclude);
        word = string.Empty;
      }
      else
        word += character;
    }
    if(!string.IsNullOrWhiteSpace(word))
      CountWord(word, wordCount, exclude);

    return Task.FromResult(wordCount);
  }

  private void CountWord(string word, WordCount wordCount, string[] exclude)
  {
    if (exclude.Contains(word, StringComparer.InvariantCultureIgnoreCase))
    {
      wordCount.Excluded++;
      return;
    }
    var firstLetter = char.ToUpperInvariant(word[0]);
    wordCount[firstLetter] = wordCount.ContainsKey(firstLetter)
      ? wordCount[firstLetter] + 1
      : 1;
  }
}