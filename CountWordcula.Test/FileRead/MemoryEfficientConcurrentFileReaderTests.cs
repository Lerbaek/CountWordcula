using CountWordcula.Backend.FileRead;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileRead;

public class MemoryEfficientConcurrentFileReaderTests : FileReaderTestsBase
{
  public MemoryEfficientConcurrentFileReaderTests(ConcurrentBlocksFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}