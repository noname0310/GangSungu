using System.Collections.Concurrent;

namespace GangSungu.Diagnostic;

public static class Diagnostics
{
    public static readonly ConcurrentBag<Diagnostic> List = new();
}
