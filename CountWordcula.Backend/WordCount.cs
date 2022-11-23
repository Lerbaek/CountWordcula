namespace CountWordcula.Backend;

/// <summary>
/// A dictionary of words and the number of times that they have been encountered, as well
/// as a total count of encountered excluded words.
/// </summary>
public class WordCount : Dictionary<string, long>
{
  /// <inheritdoc cref="WordCount"/>
  public WordCount()
  {
  }

  /// <inheritdoc cref="WordCount"/>
  /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/>
  /// whose elements are copied to the new <see cref="WordCount"/>.</param>
  /// <param name="excluded">The count of excluded words encountered.</param>
  public WordCount(IDictionary<string, long> dictionary, long excluded = 0)
    : base(dictionary)
  {
    Excluded = excluded;
  }

  /// <summary>
  /// The count of excluded words encountered.
  /// </summary>
  public long Excluded { get; set; }

  /// <summary>
  /// Combine instances of <see cref="WordCount"/> into a single instance.
  /// </summary>
  /// <param name="wordCounts"><see cref="WordCount"/> instances to combine.</param>
  /// <returns></returns>
  public static WordCount Combine(params WordCount[] wordCounts)
  {
    if (!wordCounts.Any())
      return new WordCount();

    var wordCount = new WordCount();

    foreach (var result in wordCounts)
    {
      wordCount.Excluded += result.Excluded;
      foreach (var key in result.Keys)
        if (wordCount.ContainsKey(key))
          wordCount[key] += result[key];
        else
          wordCount.Add(key, result[key]);
    }

    return wordCount;
  }
}