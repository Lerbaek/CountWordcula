﻿using BenchmarkDotNet.Attributes;
using CountWordcula.Backend.FileReader;

namespace CountWordcula.Benchmark;

[MemoryDiagnoser]
public class BenchmarkSpecification

{
  [Params(typeof(FluentFileReader), typeof(MemoryEfficientFileReader), typeof(MemoryEfficientParallelFileReader))]
  public Type FileReaderType { get; set; }

  private IFileReader FileReader =>
    (IFileReader)FileReaderType.GetConstructor(Type.EmptyTypes)
      .Invoke(Type.EmptyTypes);

  private string FilePath =>
    Path.Combine(
      Environment.CurrentDirectory,
      "Samples",
      $"Sample_{WordCount}.txt");

  [Params(
    //200
    //,
    //2000
    //,
    //5000
    //,
    10000
    )]
  public int WordCount { get; set; }


  [Benchmark]
  public IDictionary<char, long> FluentFileReaderBenchmark() =>
    FileReader.GetWordCountAsync(FilePath)
      .Result;
}