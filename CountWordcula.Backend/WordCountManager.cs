using CountWordcula.Backend.FileReader;
using CountWordcula.Backend.FileWriter;
using static CountWordcula.Backend.Register.ConfiguratonRegister;

namespace CountWordcula.Backend;

public class WordCountManager : IWordCountManager
{
  private readonly IFileReader fileReader;
  private readonly IFileWriter fileWriter;

  public WordCountManager(IFileReader fileReader, IFileWriter fileWriter)
  {
    this.fileReader = fileReader;
    this.fileWriter = fileWriter;
  }

  public async Task ExecuteAsync(
    string inputPath,
    string outputPath,
    string[]? excludedWords = null)
  {
    var inputFileNames = Directory
      .GetFiles(inputPath)
      .Where(fileName => !fileName.EndsWith(ExcludeFileName));

    excludedWords ??= await GetExcludedWordsAsync(inputPath);
    var wordCountTasks = inputFileNames.Select(fileName => fileReader.GetWordCountAsync(fileName, excludedWords));
    var wordCounts = await Task.WhenAll(wordCountTasks);

    var wordCount = WordCount.Combine(wordCounts);
    await fileWriter.WriteOutputFilesAsync(outputPath, wordCount);
  }

  public async Task<string[]> GetExcludedWordsAsync(string inputPath)
  {
    var excludeFilePath = Path.Combine(inputPath, ExcludeFileName);

    return File.Exists(excludeFilePath)
      ? await File.ReadAllLinesAsync(excludeFilePath)
      : Array.Empty<string>();
  }
}