namespace Lexer.Low.Tokens;

public readonly record struct Token
{
    public readonly TokenKind Kind;
    public readonly int Length;

    public Token(in TokenKind kind, int length)
    {
        Kind = kind;
        Length = length;
    }
}
