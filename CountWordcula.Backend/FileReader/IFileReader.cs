namespace CountWordcula.Backend.FileReader;

public interface IFileReader
{
  Task<IDictionary<char, long>> GetWordCountAsync(string fileName);
}