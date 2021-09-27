using System;

namespace GangSungu.Lexer.Low;

using Char32 = UInt32;

internal ref struct Cursor
{
    public int LenConsumed => InitialLength - _readOnlySpan.Length;
    public readonly int InitialLength;
    private ReadOnlySpan<Char32> _readOnlySpan;
    public Cursor(ReadOnlySpan<Char32> chars)
    {
        InitialLength = chars.Length;
        _readOnlySpan = chars;
    }
    public Char32 First() => Lookup(0); 
    public Char32 Second() => Lookup(1);
    public Char32 Lookup(int offset)
    {
        if (_readOnlySpan.Length <= offset)
            return '\0';
        return _readOnlySpan[offset];
    }
    public Char32? Consume()
    {
        if (_readOnlySpan.Length <= 0)
            return null;
        var next = _readOnlySpan[0];
        _readOnlySpan = _readOnlySpan[1..];
        return next;
    }
}
