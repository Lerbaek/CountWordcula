using System.Dynamic;
using GoCommando;

namespace CountWordcula.Read;

public interface ICountWords : ICommand
{
  public string InputPath { get; set; }
  public string OutputPath { get; set; }
  public string Extension { get; set; }
  public bool Segregate { get; set; }
  public bool Force { get; set; }

}