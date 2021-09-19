using System;
using System.Text;
using Char32 = System.UInt32;

namespace Lexer;

public ref struct Utf32String
{
    public readonly ReadOnlySpan<Char32> Span;
    private readonly byte[] _str;

    public unsafe Utf32String(string str)
    {
        _str = Encoding.UTF32.GetBytes(str);
        fixed (byte* strPtr = _str)
            Span = new(strPtr, _str.Length >> 2);
    }
    public override string ToString() => Encoding.UTF32.GetString(_str);
}
