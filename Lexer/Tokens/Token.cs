using System;

namespace Lexer.Tokens;

public readonly record struct Token
{
    public readonly TokenKind Kind;
    public readonly Span Span;

    public Token(in TokenKind kind, in Span span)
    {
        Kind = kind;
        Span = span;
    }
    public Token? Glue(in Token next)
    {
        TokenKind? tokenKind = Kind.Enum switch
        {
            TokenKindEnum.Dot => next.Kind.Enum switch
            {
                TokenKindEnum.Dot => TokenKind.Rng(),
                _ => null
            },
            TokenKindEnum.Rng => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.RngInclusive(),
                _ => null
            },
            TokenKindEnum.Assign => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.Eq(),
                _ => null
            },
            TokenKindEnum.Lt => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.Le(),
                TokenKindEnum.Lt => TokenKind.Shl(),
                _ => null
            },
            TokenKindEnum.Gt => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.Ge(),
                TokenKindEnum.Gt => TokenKind.Shr(),
                _ => null
            },
            TokenKindEnum.Add => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignAdd(),
                _ => null
            },
            TokenKindEnum.Sub => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignSub(),
                _ => null
            },
            TokenKindEnum.Mul => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignMul(),
                _ => null
            },
            TokenKindEnum.Div => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignDiv(),
                _ => null
            },
            TokenKindEnum.Mod => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignMod(),
                _ => null
            },
            TokenKindEnum.Shl => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignShl(),
                _ => null
            },
            TokenKindEnum.Shr => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignShr(),
                _ => null
            },
            TokenKindEnum.BitOr => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignBitOr(),
                TokenKindEnum.BitOr => TokenKind.LogOr(),
                _ => null
            },
            TokenKindEnum.BitAnd => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignBitAnd(),
                TokenKindEnum.BitAnd => TokenKind.LogAnd(),
                _ => null
            },
            TokenKindEnum.BitXor => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignBitXor(),
                _ => null
            },
            TokenKindEnum.BitNot => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.AssignBitNot(),
                _ => null
            },
            TokenKindEnum.LogNot => next.Kind.Enum switch
            {
                TokenKindEnum.Assign => TokenKind.Ne(),
                _ => null
            },
            TokenKindEnum.Comment
            or TokenKindEnum.OpenParen
            or TokenKindEnum.CloseParen
            or TokenKindEnum.OpenBrace
            or TokenKindEnum.CloseBrace
            or TokenKindEnum.OpenBracket
            or TokenKindEnum.CloseBracket
            or TokenKindEnum.Comma
            or TokenKindEnum.Colon
            or TokenKindEnum.Semicolon
            or TokenKindEnum.AssignAdd
            or TokenKindEnum.AssignSub
            or TokenKindEnum.AssignMul
            or TokenKindEnum.AssignDiv
            or TokenKindEnum.AssignMod
            or TokenKindEnum.AssignShl
            or TokenKindEnum.AssignShr
            or TokenKindEnum.AssignBitOr
            or TokenKindEnum.AssignBitAnd
            or TokenKindEnum.AssignBitXor
            or TokenKindEnum.AssignBitNot
            or TokenKindEnum.RngInclusive
            or TokenKindEnum.Eq
            or TokenKindEnum.Ne
            or TokenKindEnum.Le
            or TokenKindEnum.Ge
            or TokenKindEnum.LogOr
            or TokenKindEnum.LogAnd
            or TokenKindEnum.Literal
            or TokenKindEnum.Id => null,
            _ => throw new InvalidOperationException()
        };
        if (tokenKind == null) return null;
        return new Token(tokenKind.Value, Span.To(next.Span));
    }
}
