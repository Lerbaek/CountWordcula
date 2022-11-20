namespace CountWordcula.Backend.FileReader;

public class FluentFileReader : IFileReader
{
  public async Task<IDictionary<char, long>> GetWordCountAsync(string fileName)
  {
    var allText = await File.ReadAllTextAsync(fileName);
    return allText.Split()
      .Select(word => word.TrimEnd(',', '.'))
      .Where(word => !string.IsNullOrWhiteSpace(word))
      .GroupBy(word => char.ToUpperInvariant(word[0]))
      .ToDictionary(group => group.Key, group => group.LongCount());
  }
}