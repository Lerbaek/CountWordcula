using CountWordcula.Backend;
using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.FileWrite;
using CountWordcula.Backend.Registry;
using CountWordcula.Backend.Validate;
using Microsoft.Extensions.Logging;

namespace CountWordcula.Profiling
{
  public class Program
  {
    /// <summary>
    /// File size: 64,9 MB
    /// </summary>
    private static string SampleFileName => Path.Combine(ConfigurationRegistry.SampleInputDirectoryName, "Sample_10000000.txt");

    static async Task Main()
    {
      //await CreateGiantSampleFile(); // File size: 64,9 MB

      // Baseline memory usage: 16 MB

      IFileReader fileReader = new

        // Memory usage: 1.5 GB
        //FluentFileReader

        // Memory usage: 666 MB
        //ConcurrentLinesFileReader

        // Memory usage: 466 MB
        //ConcurrentBlocksFileReader

        // Memory usage: 17 MB
        MemoryEfficientFileReader
        ();
      
      //var result = await fileReader.GetWordCountAsync(SampleFileName);

      var manager = new WordCountManager(
        fileReader,
        new FileWriter(),
        new WordCountConfigurationValidator(ConsoleLogger<WordCountConfigurationValidator>()),
        new ExcludeFileValidator(ConsoleLogger<ExcludeFileValidator>()),
        ConsoleLogger<WordCountManager>());

      // Memory usage with MemoryEfficientFileReader: 18 MB

      await manager.RunAsync(
        ConfigurationRegistry.SampleInputDirectoryName,
        "txt",
        ConfigurationRegistry.ResultDirectoryName,
        true);
    }

    public static Logger<T> ConsoleLogger<T>() => new(LoggerFactory.Create(c => c.AddConsole()));
  }
}