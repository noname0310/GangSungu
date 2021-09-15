﻿namespace Lexer.Low.Tokens;

public readonly struct Token
{
    public readonly TokenKind Kind;
    public readonly int Length;

    public Token(TokenKind kind, int length)
    {
        Kind = kind;
        Length = length;
    }
}
