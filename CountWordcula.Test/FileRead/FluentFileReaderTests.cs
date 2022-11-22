using CountWordcula.Backend.FileRead;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileRead;

public class FluentFileReaderTests : FileReaderTestsBase
{
  public FluentFileReaderTests(FluentFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}