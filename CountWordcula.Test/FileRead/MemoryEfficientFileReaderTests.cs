using CountWordcula.Backend.FileRead;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileRead;

public class MemoryEfficientFileReaderTests : FileReaderTestsBase
{
  public MemoryEfficientFileReaderTests(MemoryEfficientFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}