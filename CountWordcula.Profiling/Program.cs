using CountWordcula.Backend.FileRead;
using CountWordcula.Backend.Registry;

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
        FluentFileReader

        // Memory usage: 666 MB
        //ConcurrentLinesFileReader

        // Memory usage: 466 MB
        //ConcurrentBlocksFileReader

        // Memory usage: 17 MB
        //MemoryEfficientFileReader
        ();
      
      _ = await fileReader.GetWordCountAsync(SampleFileName);
    }
  }
}