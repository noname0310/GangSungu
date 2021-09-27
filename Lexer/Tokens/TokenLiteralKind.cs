namespace GangSungu.Lexer.Tokens;

public enum TokenLiteralKind
{
    Bool,
    IntegerBinary,
    IntegerOctal,
    IntegerHexadecimal,
    IntegerDecimal,
    Float,
    SingleQuotedStr,
    DoubleQuotedStr,
}

public static class TokenLiteralKindExtension
{
    public static bool IsNumber(this TokenLiteralKind tokenLiteralKind) => tokenLiteralKind switch
    {
        TokenLiteralKind.IntegerBinary
        or TokenLiteralKind.IntegerOctal
        or TokenLiteralKind.IntegerHexadecimal
        or TokenLiteralKind.IntegerDecimal
        or TokenLiteralKind.Float => true,
        _ => false,
    };
}
