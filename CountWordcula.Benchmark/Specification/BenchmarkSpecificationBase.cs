using BenchmarkDotNet.Attributes;
using CountWordcula.Backend.FileRead;

namespace CountWordcula.Benchmark.Specification;

[MemoryDiagnoser]
public class BenchmarkSpecificationBase
{
  [Params(
    typeof(FluentFileReader),
    typeof(MemoryEfficientFileReader),
    typeof(ConcurrentLinesFileReader)
    //typeof(ConcurrentBlocksFileReader)
  )]
  public Type FileReaderType { get; set; } = null!;

  protected IFileReader FileReader =>
    (IFileReader)FileReaderType.GetConstructor(Type.EmptyTypes)!
      .Invoke(Type.EmptyTypes);

}