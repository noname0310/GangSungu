using System;
using System.Text;
using Char32 = System.UInt32;

namespace GangSungu.Lexer;

internal static class ReadOnlySpanExtension
{
    public static bool Compare(this ReadOnlySpan<Char32> readonlySpan, string chars)
    {
        if (readonlySpan.Length != chars.Length)
            return false;
        for (var i = 0; i < chars.Length; i++)
            if (chars[i] != readonlySpan[i])
                return false;
        return true;
    }

    public static string ToUtf16String(this ReadOnlySpan<Char32> readonlySpan)
    {
        unsafe
        {
            fixed (Char32* chars = readonlySpan)
                return Encoding.UTF32.GetString((byte*)chars, readonlySpan.Length << 2);
        }
    }
}
