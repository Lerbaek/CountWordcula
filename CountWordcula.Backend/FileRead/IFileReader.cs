namespace CountWordcula.Backend.FileRead;

public interface IFileReader
{
  Task<WordCount> GetWordCountAsync(string fileName, params string[] exclude);
}