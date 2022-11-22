using CountWordcula.Command;
using CountWordcula.Configure;
using GoCommando;

namespace CountWordcula
{
  internal class Program
  {
    private static void Main() => Go.Run<CommandFactory>();
  }
}