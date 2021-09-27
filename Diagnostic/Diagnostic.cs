using System.Collections.Concurrent;

namespace GangSungu.Diagnostic;

public struct Diagnostic
{
    public readonly Level Level;
    public readonly string Message;
    public readonly MultiSpan MultiSpan;
    public static ConcurrentBag<Diagnostic> Diagnostics => GangSungu.Diagnostic.Diagnostics.List;
    public Diagnostic(Level level, string message, MultiSpan span)
    {
        Level = level;
        Message = message;
        MultiSpan = span;
    }
    public void Register() => GangSungu.Diagnostic.Diagnostics.List.Add(this);
}
