﻿using System;
using System.Collections.Generic;
using Char32 = System.UInt32;

namespace Lexer;

public readonly ref struct Source
{
    public readonly Span Span;
    public readonly string Name;
    public readonly SourcePath Path;
    public readonly ReadOnlySpan<Char32> Content;
    public readonly List<Pos> Lines;
    public int LineNumber => Lines.Count;

    public Source(Span span, string name, SourcePath path, ReadOnlySpan<Char32> content)
    {
        Content = content;
        Lines = new(){ span.Low };
        for (var i = 0; i < Content.Length; i++)
        {
            if (Content[i] == '\n')
                Lines.Add(new Pos(span.Low + new Pos(i + 1)));
        }
        Span = span;
        Name = name;
        Path = path;
    }
    public Span LineSpan(int line) => new(Lines[line], Lines[line + 1]);
    public int FindLine(Pos pos)
    {
        var searchResult = Lines.BinarySearch(pos);
        if (0 <= searchResult)
        {
            
        }
        //TODO : NotImplementedException
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
    public ReadOnlySpan<Char32> Slice(Span span) =>
        Content[(span.Low - Span.Low)..(span.High - Span.Low)];
    public ReadOnlySpan<Char32> SliceLine(int line) => Slice(LineSpan(line)).TrimEnd('\n').TrimEnd('\r');
}
