using System;

namespace Lexer.Low.Tokens;

public readonly record struct TokenNumberLiteralKind
{
    public readonly TokenNumberLiteralKindEnum Enum { get; private init; }
    private readonly TokenIntegerLiteralKind TokenIntegerLiteralKind { get; init; }

    public TokenIntegerLiteralKind ToInteger()
    {
        if (Enum != TokenNumberLiteralKindEnum.Integer)
            throw new InvalidOperationException();
        return TokenIntegerLiteralKind;
    }
    public static TokenNumberLiteralKind Integer(TokenIntegerLiteralKind tokenIntegerLiteralKind) =>
        new() { Enum = TokenNumberLiteralKindEnum.Float, TokenIntegerLiteralKind = tokenIntegerLiteralKind };
    public static TokenNumberLiteralKind Float() => new() { Enum = TokenNumberLiteralKindEnum.Float };
}

public enum TokenNumberLiteralKindEnum : short
{
    Integer,
    Float,
}
