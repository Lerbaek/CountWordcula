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
}