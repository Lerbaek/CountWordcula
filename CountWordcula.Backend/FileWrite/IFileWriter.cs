namespace CountWordcula.Backend.FileWrite;

public interface IFileWriter
{
  Task WriteOutputFilesAsync(string outputPath, WordCount wordCount);
}