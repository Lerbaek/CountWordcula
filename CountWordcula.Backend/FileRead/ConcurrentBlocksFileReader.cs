using System.Collections.Concurrent;

namespace CountWordcula.Backend.FileRead;

/// <summary>
/// A file reader with focus on low memory usage that reads blocks of words
/// from a file and parallelizes the parsing of the text.
/// </summary>
public class ConcurrentBlocksFileReader : IFileReader
{
  private ConcurrentBag<WordCount> wordCounts = null!;

  /// <inheritdoc />
  public async Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude)
  {
    wordCounts = new ConcurrentBag<WordCount>();
    using var reader = File.OpenText(fileName);
    var block = string.Empty;
    var tasks = new List<Task>();
    while (reader.Peek() >= 0)
    {
      var character =(char)reader.Read();
      if (character is ' ' or '\r' or '\n' &&
          block.Length > 2000)
      {
        var currentBlock = block;
        tasks.Add(Task.Run(() => CountBlock(currentBlock, exclude)));
        block = string.Empty;
      }
      else
        block += character;
    }

    if(!string.IsNullOrWhiteSpace(block))
      CountBlock(block, exclude);

    await Task.WhenAll(tasks);

    return WordCount.Combine(wordCounts.ToArray());
  }

  private void CountBlock(string block, string[] exclude)
  {
    var wordCount = new WordCount();
    foreach (var word in block.Split(' ', '\r', '\n').Where(word => !string.IsNullOrEmpty(word)))
    {
      if (exclude.Contains(word, StringComparer.InvariantCultureIgnoreCase))
      {
        wordCount.Excluded++;
        continue;
      }
      wordCount[word] = wordCount.ContainsKey(word)
        ? wordCount[word] + 1
        : 1;
    }

    wordCounts.Add(wordCount);
  }
}