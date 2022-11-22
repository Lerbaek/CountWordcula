namespace CountWordcula.Backend.FileWrite;

/// <summary>
/// Write word counts to files on disk.
/// </summary>
public interface IFileWriter
{
  /// <summary>
  /// Write the contents of <paramref name="wordCount"/> to output files grouped
  /// by first letter (plus an exclude file) in <paramref name="outputPath"/>.
  /// </summary>
  /// <inheritdoc cref="WordCountConfiguration(string,string,string,bool)"/>
  /// <param name="outputPath"/>
  /// <param name="wordCount"/>
  /// <returns></returns>
  Task WriteOutputFilesAsync(string outputPath, WordCount wordCount);
}