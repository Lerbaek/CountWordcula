namespace CountWordcula.Backend.FileReader;

public interface IFileReader
{
  Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude);
}