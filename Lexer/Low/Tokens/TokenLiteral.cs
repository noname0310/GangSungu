using System.Runtime.InteropServices;

namespace Lexer.Low.Tokens;

[StructLayout(LayoutKind.Explicit)]
public readonly struct TokenLiteral
{
    [FieldOffset(0)]
    public readonly TokenLiteralKind Kind;
    [FieldOffset(sizeof(TokenLiteralKind))]
    private readonly TokenNumberLiteral _tokenNumberLiteral;
    [FieldOffset(sizeof(TokenLiteralKind))]
    private readonly TokenStrLiteral _tokenStrLiteral;

    private TokenLiteral(TokenLiteralKind kind, TokenNumberLiteral tokenNumberLiteral)
    {
        Kind = kind;
        _tokenStrLiteral = new();
        _tokenNumberLiteral = tokenNumberLiteral;
    }

    private TokenLiteral(TokenLiteralKind kind, TokenStrLiteral tokenStrLiteral)
    {
        Kind = kind;
        _tokenNumberLiteral = new();
        _tokenStrLiteral = tokenStrLiteral;
    }

    public TokenNumberLiteral ToNumberKind()
    {
        if (Kind != TokenLiteralKind.Number)
            throw new InvalidOperationException();
        return _tokenNumberLiteral;
    }

    public TokenStrLiteral ToSingleQuotedStrKind()
    {
        if (Kind != TokenLiteralKind.SingleQuotedStr)
            throw new InvalidOperationException();
        return _tokenStrLiteral;
    }

    public TokenStrLiteral ToDoubleQuotedStrKind()
    {
        if (Kind != TokenLiteralKind.DoubleQuotedStr)
            throw new InvalidOperationException();
        return _tokenStrLiteral;
    }

    public static TokenLiteral NumberKind(TokenNumberLiteral tokenNumberLiteral) => new(TokenLiteralKind.Number, tokenNumberLiteral);
    public static TokenLiteral SingleQuotedStrKind(TokenStrLiteral tokenStrLiteral) => new(TokenLiteralKind.SingleQuotedStr, tokenStrLiteral);
    public static TokenLiteral DoubleQuotedStrKind(TokenStrLiteral tokenStrLiteral) => new(TokenLiteralKind.DoubleQuotedStr, tokenStrLiteral);
}
