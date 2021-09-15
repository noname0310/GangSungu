namespace Lexer.Low.Tokens;

public readonly struct TokenNumberLiteralKind
{
    public readonly TokenNumberLiteralKindEnum Enum { get; private init; }
    private readonly TokenIntegerLiteralKind _tokenIntegerLiteralKind { get; init; }

    public TokenIntegerLiteralKind ToIntegerKind()
    {
        if (Enum != TokenNumberLiteralKindEnum.Integer)
            throw new InvalidOperationException();
        return _tokenIntegerLiteralKind;
    }
    public static TokenNumberLiteralKind Integer(TokenIntegerLiteralKind tokenIntegerLiteralKind) =>
        new() { Enum = TokenNumberLiteralKindEnum.Float, _tokenIntegerLiteralKind = tokenIntegerLiteralKind };
    public static TokenNumberLiteralKind Float() => new() { Enum = TokenNumberLiteralKindEnum.Float };
}

public enum TokenNumberLiteralKindEnum : short
{
    Integer,
    Float,
}
