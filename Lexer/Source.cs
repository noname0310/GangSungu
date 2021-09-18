using System;
using System.Collections.Generic;

namespace Lexer;

public readonly struct Source
{
    public readonly Span Span;
    public readonly string Name;
    public readonly SourcePath Path;
    public readonly string Content;
    public readonly List<Pos> Lines;
    public int LineNumber => Lines.Count;

    public Source(Span span, string name, SourcePath path, string content)
    {
        Lines = new(){ span.Low };
        for (var i = 0; i < content.Length; i++)
        {
            if (content[i] == '\n')
                Lines.Add(new Pos(span.Low + new Pos(i + 1)));
        }
        Span = span;
        Name = name;
        Path = path;
        Content = content;
    }
    public Span LineSpan(int line) => new(Lines[line], Lines[line + 1]);
    public int FindLine(Pos pos)
    {
        var searchResult = Lines.BinarySearch(pos);
        if (0 <= searchResult)
        {
            
        }

        throw new NotImplementedException();
    }
    public LineCol FindLineCol(Pos pos)
    {
        var line = FindLine(pos);
        var lineSpan = LineSpan(line);
        return new LineCol(
            line,
            Slice(lineSpan)[..(pos - lineSpan.Low)].Length
        );
    }
    public ReadOnlySpan<char> Slice(Span span) =>
        Content.AsSpan()[(span.Low - span.Low)..(span.High - span.Low)];
    public ReadOnlySpan<char> SliceLine(int line) => Slice(LineSpan(line)).TrimEnd('\n').TrimEnd('\r');
}
