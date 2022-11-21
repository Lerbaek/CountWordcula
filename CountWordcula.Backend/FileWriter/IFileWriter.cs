namespace CountWordcula.Backend.FileWriter;

public interface IFileWriter
{
  Task WriteOutputFilesAsync(string outputPath, WordCount wordCount);
}