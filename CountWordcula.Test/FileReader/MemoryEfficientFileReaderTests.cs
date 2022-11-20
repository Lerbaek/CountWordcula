using CountWordcula.Backend.FileReader;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileReader;

public class MemoryEfficientFileReaderTests : FileReaderTestsBase
{
  public MemoryEfficientFileReaderTests(MemoryEfficientFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}