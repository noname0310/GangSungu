using Lexer.Low.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lexer.Low;

using Char32 = UInt32;

public struct LexEnumerator : IEnumerator<Token>
{
    public Token Current { get; private set; }
    object IEnumerator.Current => Current;
    private ReadOnlyMemory<byte> _inputReadOnlyMemory;
    private readonly byte[] _initInput;
    public LexEnumerator(string input)
    {
        _initInput = Encoding.UTF32.GetBytes(input);
        _inputReadOnlyMemory = new(_initInput);
        Current = default;
    }
    public void Dispose() { }
    public bool MoveNext()
    {
        if (_inputReadOnlyMemory.IsEmpty)
            return false;

        Current = Next(_inputReadOnlyMemory.Span);
        _inputReadOnlyMemory = _inputReadOnlyMemory[(Current.Length << 2)..];
        return true;
    }
    public void Reset() => _inputReadOnlyMemory = new(_initInput);
    private static Token Next(ReadOnlySpan<byte> input)
    {
        var cursor = new Cursor(input);
        var consumed = cursor.Consume()!.Value;
        TokenKind tokenKind;

        if (char.IsWhiteSpace((char)consumed))//maybe bug because of loss of data
        {
            ConsumeWhile(ref cursor, x => char.IsWhiteSpace((char)x));//here is too
            tokenKind = TokenKind.Whitespace();
        }
        else if (IsIdStart(consumed))
        {
            ConsumeWhile(ref cursor, IsIdContinue);
            tokenKind = TokenKind.Id();
        }
        else if ('0' <= consumed && consumed <= '9')
        {
            var kind = ConsumeNumber(ref cursor, consumed);
            var suffix_start = cursor.LenConsumed;

            if (IsIdStart(cursor.First()))
            {
                cursor.Consume();
                ConsumeWhile(ref cursor, c => IsIdContinue(c));
            }

            tokenKind = TokenKind.Literal(TokenLiteralKind.Number(new TokenNumberLiteral(kind, suffix_start)));
        }
        else if (consumed == '#')
        {
            ConsumeWhile(ref cursor, c => c != '\n');
            tokenKind = TokenKind.Comment();
        }
        else
        {
            tokenKind = consumed switch
            {
                '(' => TokenKind.OpenParen(),
                ')' => TokenKind.CloseParen(),
                '{' => TokenKind.OpenBrace(),
                '}' => TokenKind.CloseBrace(),
                '[' => TokenKind.OpenBracket(),
                ']' => TokenKind.CloseBracket(),
                '.' => TokenKind.Dot(),
                ',' => TokenKind.Comma(),
                ':' => TokenKind.Colon(),
                ';' => TokenKind.Semicolon(),
                '=' => TokenKind.Eq(),
                '!' => TokenKind.Bang(),
                '<' => TokenKind.Lt(),
                '>' => TokenKind.Gt(),
                '+' => TokenKind.Plus(),
                '-' => TokenKind.Minus(),
                '*' => TokenKind.Star(),
                '/' => TokenKind.Slash(),
                '%' => TokenKind.Percent(),
                '|' => TokenKind.Or(),
                '&' => TokenKind.And(),
                '^' => TokenKind.Caret(),
                '~' => TokenKind.Tilde(),
                '\'' => TokenKind.Literal(
                    TokenLiteralKind.SingleQuotedStr(
                        new TokenStrLiteral(ConsumeSingleQuoted(ref cursor)))),
                '"' => TokenKind.Literal(
                    TokenLiteralKind.DoubleQuotedStr(
                        new TokenStrLiteral(ConsumeDoubleQuoted(ref cursor)))),
                _ => TokenKind.Unknown(),
            };
        }
        return new Token(tokenKind, cursor.LenConsumed);
    }

    private static void ConsumeWhile(ref Cursor cursor, Func<Char32, bool> pred)
    {
        while (pred(cursor.First()))
            cursor.Consume();
    }

    private static bool IsIdStart(Char32 c) =>
        ('a' <= c && c <= 'z')
        || ('A' <= c && c <= 'Z')
        || (c == '_')
        || (c > '\x7f' && UnicodeXid.IsXidStart(c));

    private static bool IsIdContinue(Char32 c) =>
        ('a' <= c && c <= 'z')
        || ('A' <= c && c <= 'Z')
        || ('0' <= c && c <= '9')
        || (c == '_')
        || (c > '\x7f' && UnicodeXid.IsXidContinue(c));

    private static TokenNumberLiteralKind ConsumeNumber(ref Cursor cursor, Char32 firstChar)
    {
        TokenIntegerLiteralKind kind;
        Char32 first;
        Char32 second;
        if (firstChar == '0')
        {
            first = cursor.First();
            second = cursor.Second();
            if (first == 'b' && second.IsBinary())
            {
                cursor.Consume();
                ConsumeWhile(ref cursor, x => x.IsBinary());
                kind = TokenIntegerLiteralKind.Binary;
            }
            else if (first == 'o' && second.IsOctal())
            {
                cursor.Consume();
                ConsumeWhile(ref cursor, x => x.IsOctal());
                kind = TokenIntegerLiteralKind.Octal;
            }
            else if (first == 'x' && second.IsHexical())
            {
                cursor.Consume();
                ConsumeWhile(ref cursor, x => x.IsHexical());
                kind = TokenIntegerLiteralKind.Hexadecimal;
            }
            else if ('0' <= first && first <= '9')
            {
                cursor.Consume();
                ConsumeWhile(ref cursor, x => x.IsDecimal());
                kind = TokenIntegerLiteralKind.Decimal;
            }
            else if (first == '.' || first == 'e' || first == 'E')
                kind = TokenIntegerLiteralKind.Decimal;
            else
                return TokenNumberLiteralKind.Integer(TokenIntegerLiteralKind.Decimal);
        }
        else
            kind = TokenIntegerLiteralKind.Decimal;

        if (kind != TokenIntegerLiteralKind.Decimal)
            return TokenNumberLiteralKind.Integer(kind);

        first = cursor.First();
        second = cursor.Second();

        if (first == '.' && second.IsDecimal())
        {
            cursor.Consume();
            ConsumeWhile(ref cursor, x => x.IsDecimal());

            if ((cursor.First() is 'e' or 'E') && (cursor.Second() is '+' or '-') && cursor.Lookup(2).IsDecimal())
            {
                cursor.Consume();
                cursor.Consume();
                ConsumeWhile(ref cursor, x => x.IsDecimal());
            }
            else if ((cursor.First() is 'e' or 'E') && cursor.Second().IsDecimal()) {
                cursor.Consume();
                ConsumeWhile(ref cursor, x => x.IsDecimal());
            }
            return TokenNumberLiteralKind.Float();
        }
        if ((first is 'e' or 'E') &&
            (
                ((cursor.Second() is '+' or '-') && cursor.Lookup(2).IsDecimal()) 
                || cursor.Second().IsDecimal()
            ))
        {
            cursor.Consume();
            if (cursor.First() is '+' or '-')
                cursor.Consume();

            ConsumeWhile(ref cursor, x => x.IsDecimal());
            return TokenNumberLiteralKind.Float();
        }
        else
        {
            ConsumeWhile(ref cursor, x => x.IsDecimal());
            return TokenNumberLiteralKind.Integer(TokenIntegerLiteralKind.Decimal);
        }
    }

    private static bool ConsumeSingleQuoted(ref Cursor cursor)
    {
        // Normal case e.g. 'a'
        if (cursor.First() != '\\' && cursor.Second() == '\'')
        {
            cursor.Consume();
            cursor.Consume();
            return true;
        }
        for (; ;) 
        {
            switch (cursor.First())
            {
                case '\'': 
                    cursor.Consume();
                    return true;
                case '\\':
                    cursor.Consume();
                    cursor.Consume();
                    break;
                case '#': goto loopOut;
                case '\0': goto loopOut;
                case '\n': 
                    if (cursor.Second() != '\'') 
                        goto loopOut;
                    goto default;
                default:
                    cursor.Consume();
                    break;
            }
        }
        loopOut:
        return false;
    }

    private static bool ConsumeDoubleQuoted(ref Cursor cursor)
    {
        for (; ; ) 
        {
            var current = cursor.Consume();
            if (current == null) break; 
            switch (current)
            {
                case '\"': return true;
                case '\\': cursor.Consume(); break;
            }
        }
        return false;
    }
}
