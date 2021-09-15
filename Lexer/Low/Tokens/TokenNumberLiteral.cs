namespace Lexer.Low.Tokens;

public readonly struct TokenNumberLiteral
{
    public readonly int SuffixStart;
    public readonly TokenNumberLiteralKind Kind;
    private readonly TokenIntegerLiteralKind _tokenIntegerLiteralKind;

    private TokenNumberLiteral(
        TokenNumberLiteralKind kind, 
        int suffixStart, 
        TokenIntegerLiteralKind tokenIntegerLiteralKind = TokenIntegerLiteralKind.Binary)
    {
        Kind = kind;
        SuffixStart = suffixStart;
        _tokenIntegerLiteralKind = tokenIntegerLiteralKind;
    }

    public TokenIntegerLiteralKind ToIntegerKind()
    {
        if (Kind != TokenNumberLiteralKind.Integer)
            throw new InvalidOperationException();
        return _tokenIntegerLiteralKind;
    }

    public static TokenNumberLiteral IntegerKind(TokenIntegerLiteralKind tokenIntegerLiteralKind, int suffixStart) 
        => new(TokenNumberLiteralKind.Integer, suffixStart, tokenIntegerLiteralKind);

    public static TokenNumberLiteral FloatKind(int suffixStart) => new(TokenNumberLiteralKind.Float, suffixStart);
}
