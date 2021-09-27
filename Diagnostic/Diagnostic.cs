using System.Collections.Concurrent;

namespace GangSungu.Diagnostic;

public struct DiagnosticItem
{
    public readonly Level Level;
    public readonly string Message;
    public readonly MultiSpan MultiSpan;
    public static ConcurrentBag<DiagnosticItem> Diagnostics => Diagnostic.Diagnostics.List;
    public DiagnosticItem(Level level, string message, MultiSpan span)
    {
        Level = level;
        Message = message;
        MultiSpan = span;
    }
    public void Register() => Diagnostic.Diagnostics.List.Add(this);
}
