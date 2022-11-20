﻿using System.Collections;

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
    return SampleDataWordCounts.Select(count => new[]{Path.Combine(Environment.CurrentDirectory, "Samples", $"Sample_{count}.txt")}).GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}