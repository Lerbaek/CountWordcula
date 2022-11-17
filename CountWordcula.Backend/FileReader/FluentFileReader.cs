namespace CountWordcula.Backend.FileReader;

public class FluentFileReader : IFileReader
{
  public async Task<IDictionary<string, long>> GetWordCountAsync(string fileName)
  {
    var allText = await File.ReadAllTextAsync(fileName);
    return allText.Split()
      .Select(word => word.TrimEnd(',', '.'))
      .GroupBy(word => word.ToUpperInvariant())
      .ToDictionary(group => group.Key, group => group.LongCount());
  }
}