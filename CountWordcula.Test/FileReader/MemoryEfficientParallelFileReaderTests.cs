using CountWordcula.Backend.FileReader;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileReader;

public class MemoryEfficientParallelFileReaderTests : FileReaderTestsBase
{
  public MemoryEfficientParallelFileReaderTests(MemoryEfficientParallelFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}