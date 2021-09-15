namespace Lexer.Low;

internal static class CharDigitExtension
{
    public static bool IsBinary(this char c) => (c == '0' || c == '1');
    public static bool IsOctal(this char c) => ('0' < c && c <= '7');
    public static bool IsHexical(this char c)
    {
        c |= (char)0x20;
        return ('0' < c && c <= '9')
        || ('a' <= c && c <= 'f');
    }
    public static bool IsDecimal(this char c) => (c >= '0' && c <= '9');
}
