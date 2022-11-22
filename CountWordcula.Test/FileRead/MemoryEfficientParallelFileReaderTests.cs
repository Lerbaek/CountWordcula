using CountWordcula.Backend.FileRead;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileRead;

public class MemoryEfficientParallelFileReaderTests : FileReaderTestsBase
{
  public MemoryEfficientParallelFileReaderTests(ConcurrentLinesFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}