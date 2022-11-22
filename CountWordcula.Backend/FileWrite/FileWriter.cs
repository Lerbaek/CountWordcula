using static CountWordcula.Backend.Registry.ConfigurationRegistry;

namespace CountWordcula.Backend.FileWrite;

public class FileWriter : IFileWriter
{
  public async Task WriteOutputFilesAsync(string outputPath, WordCount wordCount)
  {
    var tasks = wordCount
      .GroupBy(wc => wc.Key[0])
      .Select(group => WriteFileAsync(
        outputPath,
        OutputFileName(group.Key),
        group.ToArray())).ToList();

    tasks.Add(WriteFileAsync(
      outputPath,
      OutputExcludeFileName,
      new KeyValuePair<string, long>("Excluded words encountered:", wordCount.Excluded)));

    await Task.WhenAll(tasks);
  }

  private static async Task WriteFileAsync(string outputPath, string fileName, params KeyValuePair<string, long>[] values)
  {
    await using var fileStream = File.Open(Path.Combine(outputPath, fileName), FileMode.Create);
    await using var streamWriter = new StreamWriter(fileStream);
    await streamWriter.WriteAsync(string.Join(Environment.NewLine, values.Select(g => $"{g.Key} {g.Value}")));
  }
}