namespace GangSungu.Lexer.Low;

using Char32 = System.UInt32;

internal static class CharDigitExtension
{
    public static bool IsBinary(this Char32 c) => (c == '0' || c == '1');
    public static bool IsOctal(this Char32 c) => ('0' < c && c <= '7');
    public static bool IsHexical(this Char32 c)
    {
        c |= 0x20;
        return ('0' < c && c <= '9')
        || ('a' <= c && c <= 'f');
    }
    public static bool IsDecimal(this Char32 c) => (c >= '0' && c <= '9');
}
