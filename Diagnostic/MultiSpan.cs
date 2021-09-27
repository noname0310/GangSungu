using Lexer;

namespace Diagnostic;

public struct MultiSpan
{
    public readonly List<(string, Span?)> Spans;
    public MultiSpan() => Spans = new();
    public MultiSpan(List<(string, Span?)> spans) => Spans = spans;
}
