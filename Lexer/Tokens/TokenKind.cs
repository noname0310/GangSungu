using System;
using System.Runtime.InteropServices;

namespace Lexer.Tokens;

[StructLayout(LayoutKind.Explicit)]
public readonly struct TokenKind
{
    public TokenKindEnum Enum { get => _enum; private init => _enum = value; }
    private Symbol Symbol { init => _symbol = value; }
    private TokenLiteral TokenLiteral { init => _tokenLiteral = value; }

    [FieldOffset(0)]
    private readonly TokenKindEnum _enum;
    [FieldOffset(sizeof(TokenKindEnum))]
    private readonly Symbol _symbol;
    [FieldOffset(sizeof(TokenKindEnum))]
    private readonly TokenLiteral _tokenLiteral;

    public Symbol ToId()
    {
        if (Enum != TokenKindEnum.Id)
            throw new InvalidOperationException();
        return Symbol;
    }
    public TokenLiteral ToLiteral()
    {
        if (Enum != TokenKindEnum.Literal)
            throw new InvalidOperationException();
        return _tokenLiteral;
    }
    public override string ToString() => _enum switch
    {
        TokenKindEnum.Comment => "comment",
        TokenKindEnum.OpenParen => "'('",
        TokenKindEnum.CloseParen => "')'",
        TokenKindEnum.OpenBrace => "'{'",
        TokenKindEnum.CloseBrace => "'}'",
        TokenKindEnum.OpenBracket => "'['",
        TokenKindEnum.CloseBracket => "']'",
        TokenKindEnum.Dot => "'.'",
        TokenKindEnum.Comma => "','",
        TokenKindEnum.Colon => "':'",
        TokenKindEnum.Semicolon => "';'",
        TokenKindEnum.Assign => "'='",
        TokenKindEnum.AssignAdd => "'+='",
        TokenKindEnum.AssignSub => "'-='",
        TokenKindEnum.AssignMul => "'*='",
        TokenKindEnum.AssignDiv => "'/='",
        TokenKindEnum.AssignMod => "'%='",
        TokenKindEnum.AssignShl => "'<<='",
        TokenKindEnum.AssignShr => "'>>='",
        TokenKindEnum.AssignBitOr => "'|='",
        TokenKindEnum.AssignBitAnd => "'&='",
        TokenKindEnum.AssignBitXor => "'^='",
        TokenKindEnum.AssignBitNot => "'~='",
        TokenKindEnum.Rng => "'..'",
        TokenKindEnum.RngInclusive => "'..='",
        TokenKindEnum.Eq => "'=='",
        TokenKindEnum.Ne => "'!='",
        TokenKindEnum.Lt => "'<'",
        TokenKindEnum.Gt => "'>'",
        TokenKindEnum.Le => "'<='",
        TokenKindEnum.Ge => "'>='",
        TokenKindEnum.Add => "'+'",
        TokenKindEnum.Sub => "'-'",
        TokenKindEnum.Mul => "'*'",
        TokenKindEnum.Div => "'/'",
        TokenKindEnum.Mod => "'%'",
        TokenKindEnum.Shl => "'<<'",
        TokenKindEnum.Shr => "'>>'",
        TokenKindEnum.BitOr => "'|'",
        TokenKindEnum.BitAnd => "'&'",
        TokenKindEnum.BitXor => "'^'",
        TokenKindEnum.LogOr => "'||'",
        TokenKindEnum.LogAnd => "'&&'",
        TokenKindEnum.BitNot => "'~'",
        TokenKindEnum.LogNot => "'!'",
        TokenKindEnum.Id => "identifier",
        TokenKindEnum.Literal => "literal",
        _ => throw new InvalidOperationException(),
    };
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
    public static TokenKind Assign() => new() { Enum = TokenKindEnum.Assign };
    public static TokenKind AssignAdd() => new() { Enum = TokenKindEnum.AssignAdd };
    public static TokenKind AssignSub() => new() { Enum = TokenKindEnum.AssignSub };
    public static TokenKind AssignMul() => new() { Enum = TokenKindEnum.AssignMul };
    public static TokenKind AssignDiv() => new() { Enum = TokenKindEnum.AssignDiv };
    public static TokenKind AssignMod() => new() { Enum = TokenKindEnum.AssignMod };
    public static TokenKind AssignShl() => new() { Enum = TokenKindEnum.AssignShl };
    public static TokenKind AssignShr() => new() { Enum = TokenKindEnum.AssignShr };
    public static TokenKind AssignBitOr() => new() { Enum = TokenKindEnum.AssignBitOr };
    public static TokenKind AssignBitAnd() => new() { Enum = TokenKindEnum.AssignBitAnd };
    public static TokenKind AssignBitXor() => new() { Enum = TokenKindEnum.AssignBitXor };
    public static TokenKind AssignBitNot() => new() { Enum = TokenKindEnum.AssignBitNot };
    public static TokenKind Rng() => new() { Enum = TokenKindEnum.Rng };
    public static TokenKind RngInclusive() => new() { Enum = TokenKindEnum.RngInclusive };
    public static TokenKind Eq() => new() { Enum = TokenKindEnum.Eq };
    public static TokenKind Ne() => new() { Enum = TokenKindEnum.Ne };
    public static TokenKind Lt() => new() { Enum = TokenKindEnum.Lt };
    public static TokenKind Gt() => new() { Enum = TokenKindEnum.Gt };
    public static TokenKind Le() => new() { Enum = TokenKindEnum.Le };
    public static TokenKind Ge() => new() { Enum = TokenKindEnum.Ge };
    public static TokenKind Add() => new() { Enum = TokenKindEnum.Add };
    public static TokenKind Sub() => new() { Enum = TokenKindEnum.Sub };
    public static TokenKind Mul() => new() { Enum = TokenKindEnum.Mul };
    public static TokenKind Div() => new() { Enum = TokenKindEnum.Div };
    public static TokenKind Mod() => new() { Enum = TokenKindEnum.Mod };
    public static TokenKind Shl() => new() { Enum = TokenKindEnum.Shl };
    public static TokenKind Shr() => new() { Enum = TokenKindEnum.Shr };
    public static TokenKind BitOr() => new() { Enum = TokenKindEnum.BitOr };
    public static TokenKind BitAnd() => new() { Enum = TokenKindEnum.BitAnd };
    public static TokenKind BitXor() => new() { Enum = TokenKindEnum.BitXor };
    public static TokenKind LogOr() => new() { Enum = TokenKindEnum.LogOr };
    public static TokenKind LogAnd() => new() { Enum = TokenKindEnum.LogAnd };
    public static TokenKind BitNot() => new() { Enum = TokenKindEnum.BitNot };
    public static TokenKind LogNot() => new() { Enum = TokenKindEnum.LogNot };
    public static TokenKind Id(Symbol symbol) => new() { Enum = TokenKindEnum.Id, Symbol = symbol };
    public static TokenKind Id(string id) => new() { Enum = TokenKindEnum.Id, Symbol = new Symbol(string.Intern(id)) };
    public static TokenKind Literal(in TokenLiteral tokenLiteral) => new() { Enum = TokenKindEnum.Literal, TokenLiteral = tokenLiteral };
}

public enum TokenKindEnum : short
{
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
    // Assignment operators
    Assign,       // "="
    AssignAdd,    // "+="
    AssignSub,    // "-="
    AssignMul,    // "*="
    AssignDiv,    // "/="
    AssignMod,    // "%="
    AssignShl,    // "<<="
    AssignShr,    // ">>="
    AssignBitOr,  // "|="
    AssignBitAnd, // "&="
    AssignBitXor, // "^="
    AssignBitNot, // "~="
    // Range operators
    Rng,          // ".."
    RngInclusive, // "..="
    // Cmp operators
    Eq, // "=="
    Ne, // "!="
    Lt, // "<"
    Gt, // ">"
    Le, // "<="
    Ge, // ">="
    // Binary operators
    Add,    // "+"
    Sub,    // "-"
    Mul,    // "*"
    Div,    // "/"
    Mod,    // "%"
    Shl,    // "<<"
    Shr,    // ">>"
    BitOr,  // "|"
    BitAnd, // "&"
    BitXor, // "^"
    LogOr,  // "||"
    LogAnd, // "&&"
    // Unary operators
    BitNot, // "~"
    LogNot, // "!"
    Id,
    Literal
}
