using System.Collections;
using static CountWordcula.Backend.Register.ConfigurationRegister;

namespace CountWordcula.Test.TestData;

public class SamplePathProvider : IEnumerable<object[]>
{
  private int[] SampleDataWordCounts =>
    new[]
    {
      200,
      2000,
      5000,
      10000
    };
  public IEnumerator<object[]> GetEnumerator()
  {
    return SampleDataWordCounts.Select(count => new[]{Path.Combine(Environment.CurrentDirectory, SampleInputDirectoryName, $"Sample_{count}.txt")}).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}