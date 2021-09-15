using System;

namespace Lexer.Low;

using Char32 = UInt32;

internal ref struct Cursor
{
    public int LenConsumed => InitialLength - (_readOnlySpan.Length >> 2);
    public readonly int InitialLength;
    private ReadOnlySpan<byte> _readOnlySpan;
    public Cursor(ReadOnlySpan<byte> chars)
    {
        InitialLength = chars.Length >> 2;
        _readOnlySpan = chars;
    }
    public Char32 First() => Lookup(0); 
    public Char32 Second() => Lookup(1);
    public Char32 Lookup(int offset)
    {
        if (_readOnlySpan.Length <= (offset << 2))
            return '\0';
        return Read(offset);
    }
    public Char32? Consume()
    {
        if (_readOnlySpan.Length <= 0)
            return null;
        var next = Read(0);
        _readOnlySpan = _readOnlySpan[4..];
        return next;
    }
    private unsafe Char32 Read(int index)
    {
        fixed (byte* span = _readOnlySpan)
        {
            var spanInt = (Char32*)span;
            return spanInt[index];
        }
    }
}
