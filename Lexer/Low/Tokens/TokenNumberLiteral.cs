namespace Lexer.Low.Tokens;

public readonly struct TokenNumberLiteral
{
    public readonly int SuffixStart;
    public readonly TokenNumberLiteralKind Kind;
    public TokenNumberLiteral(TokenNumberLiteralKind kind, int suffixStart)
    {
        Kind = kind;
        SuffixStart = suffixStart;
    }
}
