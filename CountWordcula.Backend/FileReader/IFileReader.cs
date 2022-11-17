namespace CountWordcula.Backend.FileReader;

public interface IFileReader
{
  Task<IDictionary<string, long>> GetWordCountAsync(string fileName);
}