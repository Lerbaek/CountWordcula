using CountWordcula.Backend.FileReader;

namespace CountWordcula.Test.FileReader;

public class MemoryEfficientParallelFileReaderTests : FileReaderTestsBase
{
  public MemoryEfficientParallelFileReaderTests(MemoryEfficientParallelFileReader uut)
    : base(uut)
  {
  }
}