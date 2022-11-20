namespace CountWordcula.Backend;

public class WordCount : Dictionary<char, long>
{
  public WordCount()
  {
  }

  public WordCount(IDictionary<char, long> dictionary, long excluded = 0)
    : base(dictionary)
  {
    Excluded = excluded;
  }

  public long Excluded { get; set; }
}