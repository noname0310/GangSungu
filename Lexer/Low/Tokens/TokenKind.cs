using System;

namespace Lexer.Low.Tokens;

public readonly struct TokenKind
{
    public readonly TokenKindEnum Enum { get; private init; }
    private readonly TokenLiteralKind TokenLiteralKind { get; init; }
    public TokenLiteralKind ToTokenLiteralKind()
    {
        if (Enum != TokenKindEnum.Literal)
            throw new InvalidOperationException();
        return TokenLiteralKind;
    }
    public static TokenKind Unknown() => new() { Enum = TokenKindEnum.Unknown };
    public static TokenKind Whitespace() => new() { Enum = TokenKindEnum.Whitespace };
    public static TokenKind Comment() => new() { Enum = TokenKindEnum.Comment };
    public static TokenKind OpenParen() => new() { Enum = TokenKindEnum.OpenParen };
    public static TokenKind CloseParen() => new() { Enum = TokenKindEnum.CloseParen };
    public static TokenKind OpenBrace() => new() { Enum = TokenKindEnum.OpenBrace };
    public static TokenKind CloseBrace() => new() { Enum = TokenKindEnum.CloseBrace };
    public static TokenKind OpenBracket() => new() { Enum = TokenKindEnum.OpenBracket };
    public static TokenKind CloseBracket() => new() { Enum = TokenKindEnum.CloseBracket };
    public static TokenKind Dot() => new() { Enum = TokenKindEnum.Dot };
    public static TokenKind Comma() => new() { Enum = TokenKindEnum.Comma };
    public static TokenKind Colon() => new() { Enum = TokenKindEnum.Colon };
    public static TokenKind Semicolon() => new() { Enum = TokenKindEnum.Semicolon };
    public static TokenKind Eq() => new() { Enum = TokenKindEnum.Eq };
    public static TokenKind Bang() => new() { Enum = TokenKindEnum.Bang };
    public static TokenKind Lt() => new() { Enum = TokenKindEnum.Lt };
    public static TokenKind Gt() => new() { Enum = TokenKindEnum.Gt };
    public static TokenKind Plus() => new() { Enum = TokenKindEnum.Plus };
    public static TokenKind Minus() => new() { Enum = TokenKindEnum.Minus };
    public static TokenKind Star() => new() { Enum = TokenKindEnum.Star };
    public static TokenKind Slash() => new() { Enum = TokenKindEnum.Slash };
    public static TokenKind Percent() => new() { Enum = TokenKindEnum.Percent };
    public static TokenKind Or() => new() { Enum = TokenKindEnum.Or };
    public static TokenKind And() => new() { Enum = TokenKindEnum.And };
    public static TokenKind Caret() => new() { Enum = TokenKindEnum.Caret };
    public static TokenKind Tilde() => new() { Enum = TokenKindEnum.Tilde };
    public static TokenKind Id() => new() { Enum = TokenKindEnum.Id };
    public static TokenKind Literal(TokenLiteralKind tokenLiteralKind) =>
        new() { Enum = TokenKindEnum.Literal, TokenLiteralKind = tokenLiteralKind };
}

public enum TokenKindEnum : short
{
    Unknown,
    Whitespace,
    Comment,      // "#"
    OpenParen,    // "("
    CloseParen,   // ")"
    OpenBrace,    // "{"
    CloseBrace,   // "}"
    OpenBracket,  // "["
    CloseBracket, // "]"
    Dot,          // "."
    Comma,        // ","
    Colon,        // ":"
    Semicolon,    // ";"
    Eq,           // "="
    Bang,         // "!"
    Lt,           // "<"
    Gt,           // ">"
    Plus,         // "+"
    Minus,        // "-"
    Star,         // "*"
    Slash,        // "/"
    Percent,      // "%"
    Or,           // "|"
    And,          // "&"
    Caret,        // "^"
    Tilde,        // "~"
    Id,           // identifier or keyword
    Literal
}
