using GoCommando;

namespace CountWordcula.Read;

[Command("count-words")]
[Description("Read data from multiple files in a given directory.")]
public class CountWords : ICountWords
{
  [Parameter("input-path", "i")]
  [Description("Where are the input files?")]
  public string InputPath { get; set; } = null!;

  [Parameter("output-path", "o")]
  [Description("Where should the output files be placed?")]
  public string OutputPath { get; set; } = null!;

  [Parameter("extension", "e", true, "txt")]
  [Description("Where should the output files be placed?")]
  public string Extension { get; set; } = null!;


  [Parameter("segregate", "s", true)]
  [Description("Should there be separate output files per input file?")]
  public bool Segregate { get; set; }

  [Parameter("force", "f", true)]
  [Description("Should any existing files be overwritten?")]
  public bool Force { get; set; }

  public void Run()
  {
    throw new NotImplementedException();
  }
}