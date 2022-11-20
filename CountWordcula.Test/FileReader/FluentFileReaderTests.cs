using CountWordcula.Backend.FileReader;
using Xunit.Abstractions;

namespace CountWordcula.Test.FileReader;

public class FluentFileReaderTests : FileReaderTestsBase
{
  public FluentFileReaderTests(FluentFileReader uut, ITestOutputHelper output)
    : base(uut, output)
  {
  }
}