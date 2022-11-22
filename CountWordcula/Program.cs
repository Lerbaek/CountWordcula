using CountWordcula.Command;
using GoCommando;

namespace CountWordcula
{
  internal class Program
  {
    private static void Main() => Go.Run<CommandFactory>();
  }
}