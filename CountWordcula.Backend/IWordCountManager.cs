namespace CountWordcula.Backend;

public interface IWordCountManager
{
  Task ExecuteAsync(
    string inputPath,
    string outputPath,
    string[]? excludedWords = null);
  Task<string[]> GetExcludedWordsAsync(string inputPath);
}