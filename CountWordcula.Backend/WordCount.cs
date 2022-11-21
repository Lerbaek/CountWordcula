namespace CountWordcula.Backend;

public class WordCount : Dictionary<string, long>
{
  public WordCount()
  {
  }

  public WordCount(IDictionary<string, long> dictionary, long excluded = 0)
    : base(dictionary)
  {
    Excluded = excluded;
  }

  public long Excluded { get; set; }

  public static WordCount Combine(params WordCount[] wordCounts)
  {
    if (!wordCounts.Any())
      return new WordCount();

    var wordCount = wordCounts.First();

    foreach (var result in wordCounts.Skip(1))
    {
      wordCount.Excluded += result.Excluded;
      foreach (var key in result.Keys)
      {
        if (wordCount.ContainsKey(key))
          wordCount[key] += result[key];
        else
          wordCount.Add(key, result[key]);
      }
    }

    return wordCount;
  }
}