namespace GangSungu.Diagnostic;

using GangSungu.Span;

public struct MultiSpan
{
    public readonly List<(string, Span?)> Spans;
    public MultiSpan() => Spans = new();
    public MultiSpan(List<(string, Span?)> spans) => Spans = spans;
}
