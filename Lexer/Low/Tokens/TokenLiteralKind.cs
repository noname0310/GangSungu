using System;
using System.Runtime.InteropServices;

namespace Lexer.Low.Tokens;

[StructLayout(LayoutKind.Explicit)]
public readonly record struct TokenLiteralKind
{
    public TokenLiteralKindEnum Enum { get => _enum; private init => _enum = value; }
    private TokenNumberLiteral TokenNumberLiteral { init => _tokenNumberLiteral = value; }
    private TokenStrLiteral TokenStrLiteral { init => _tokenStrLiteral = value; }

    [FieldOffset(0)]
    private readonly TokenLiteralKindEnum _enum;
    [FieldOffset(sizeof(TokenLiteralKindEnum))]
    private readonly TokenNumberLiteral _tokenNumberLiteral;
    [FieldOffset(sizeof(TokenLiteralKindEnum))]
    private readonly TokenStrLiteral _tokenStrLiteral;
    
    public TokenNumberLiteral ToNumberKind()
    {
        if (Enum != TokenLiteralKindEnum.Number)
            throw new InvalidOperationException();
        return _tokenNumberLiteral;
    }
    public TokenStrLiteral ToSingleQuotedStrKind()
    {
        if (Enum != TokenLiteralKindEnum.SingleQuotedStr)
            throw new InvalidOperationException();
        return _tokenStrLiteral;
    }
    public TokenStrLiteral ToDoubleQuotedStrKind()
    {
        if (Enum != TokenLiteralKindEnum.DoubleQuotedStr)
            throw new InvalidOperationException();
        return _tokenStrLiteral;
    }
    public static TokenLiteralKind Number(TokenNumberLiteral tokenNumberLiteral) => 
        new() { Enum = TokenLiteralKindEnum.Number, TokenNumberLiteral = tokenNumberLiteral };
    public static TokenLiteralKind SingleQuotedStr(TokenStrLiteral tokenStrLiteral) => 
        new() { Enum = TokenLiteralKindEnum.SingleQuotedStr, TokenStrLiteral = tokenStrLiteral };
    public static TokenLiteralKind DoubleQuotedStr(TokenStrLiteral tokenStrLiteral) => 
        new() { Enum = TokenLiteralKindEnum.DoubleQuotedStr, TokenStrLiteral = tokenStrLiteral };
}

public enum TokenLiteralKindEnum : short
{
    Number,
    SingleQuotedStr,
    DoubleQuotedStr
}
