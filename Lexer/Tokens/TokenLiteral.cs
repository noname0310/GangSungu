namespace Lexer.Tokens;

public readonly struct TokenLiteral
{
    public readonly TokenLiteralKind Kind;
    public readonly Symbol Str;
    public readonly Symbol? Suffix;

    public TokenLiteral(TokenLiteralKind tokenLiteralKind, Symbol str, Symbol? suffix)
    {
        Kind = tokenLiteralKind;
        Str = str;
        Suffix = suffix;
    }
}
