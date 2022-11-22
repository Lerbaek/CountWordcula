namespace CountWordcula.Backend;

public interface IWordCountManager
{
  Task<bool> RunAsync(WordCountConfiguration configuration);
  Task<bool> RunAsync(string inputPath, string inputExtension, string outputPath, bool force = false);
}