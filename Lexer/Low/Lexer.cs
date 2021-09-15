using Lexer.Low.Tokens;
using System.Collections;

namespace Lexer.Low;

public struct Lexer : IEnumerator<Token>
{
    public Token Current { get; private set; }
    object IEnumerator.Current => Current;
    private ReadOnlyMemory<char> _inputReadOnlyMemory;
    private readonly ReadOnlyMemory<char> _initReadOnlyMemory;
    public Lexer(ReadOnlyMemory<char> input)
    {
        _inputReadOnlyMemory = input;
        _initReadOnlyMemory = input;
        Current = new();
    }
    public void Dispose() { }
    public bool MoveNext()
    {
        if (_inputReadOnlyMemory.IsEmpty)
            return false;

        Current = Next(_inputReadOnlyMemory.Span);
        _inputReadOnlyMemory = _inputReadOnlyMemory[Current.Length..];
        return true;
    }
    public void Reset() => _inputReadOnlyMemory = _initReadOnlyMemory;
    private Token Next(ReadOnlySpan<char> input)
    {
        var cursor = new Cursor(input);
        char consumed = cursor.Consume()!.Value;
        if (char.IsWhiteSpace(consumed))
        {
            ConsumeWhile(ref cursor, char.IsWhiteSpace);
            return Token.WhitespaceKind(cursor.LenConsumed);
        }
        else if (IsIdStart(consumed))
        {
            ConsumeWhile(ref cursor, IsIdContinue);
            return Token.IdKind(cursor.LenConsumed);
        }
        else if ('0' <= consumed && consumed <= '9')
        {
            let kind = consume_number(&mut cursor, char);
            let suffix_start = cursor.len_consumed();

            if is_id_start(cursor.first()) {
                cursor.consume();
                consume_while(&mut cursor, | char | is_id_continue(char));
            }

            return Token.LiteralKind(
                cursor.LenConsumed,
                TokenLiteral.NumberKind(TokenNumberLiteral.IntegerKind(suffix_start),
                Kind::Literal(TokenLiteralKind::Number(TokenNumberLiteral::new(
                kind,
                suffix_start,
            )));
        }
        '#' => {
            consume_while(&mut cursor, | char | char != '\n');
            TokenKind::Comment
        }
    }

    private void ConsumeWhile(ref Cursor cursor, Func<char, bool> pred)
    {
        while (pred(cursor.First()))
            cursor.Consume();
    }

    private bool IsIdStart(char c) =>
        ('a' <= c && c <= 'z')
        || ('A' <= c && c <= 'Z')
        || (c == '_')
        //|| (c > '\x7f' && UnicodeXID::is_xid_start(c));

    private bool IsIdContinue(char c)
    {
        ('a' <= c && c <= 'z')
        || ('A' <= c && c <= 'Z')
        || ('0' <= c && c <= '9')
        || (c == '_')
        //|| (char > '\x7f' && UnicodeXID::is_xid_continue(char))
    }

    TokenNumberLiteralKind ConsumeNumber(ref Cursor cursor, first_char: char)
    {
        let kind = if first_char == '0' {
            match cursor.first()
    {
        'b' if cursor.second().is_digit(2) =>
        {
            cursor.consume();
            consume_while(cursor, | char | char.is_digit(2));
            TokenIntegerLiteralKind::Binary
                }
                'o' if cursor.second().is_digit(8) =>
                {
                    cursor.consume();
                    consume_while(cursor, | char | char.is_digit(8));
                    TokenIntegerLiteralKind::Octal
                }
                'x' if cursor.second().is_digit(16) =>
                {
                    cursor.consume();
                    consume_while(cursor, | char | char.is_digit(16));
                    TokenIntegerLiteralKind::Hexadecimal
                }
                '0'.. = '9' => {
            cursor.consume();
            consume_while(cursor, | char | char.is_digit(10));
            TokenIntegerLiteralKind::Decimal
                  }
        '.' | 'e' | 'E' => TokenIntegerLiteralKind::Decimal,
                _ => return TokenNumberLiteralKind::Integer(TokenIntegerLiteralKind::Decimal),
            }
}
        else
        {
            TokenIntegerLiteralKind::Decimal
        };

if kind != TokenIntegerLiteralKind::Decimal {
    return TokenNumberLiteralKind::Integer(kind);
}

match cursor.first()
{
    '.' if cursor.second().is_digit(10) =>
    {
        cursor.consume();
        consume_while(cursor, | char | char.is_digit(10));

        match(cursor.first(), cursor.second(), cursor.lookup(2)) {
            ('e' | 'E', '+' | '-', digit) if digit.is_digit(10) =>
            {
                cursor.consume();
                cursor.consume();
                consume_while(cursor, | char | char.is_digit(10));
            }
            ('e' | 'E', digit, _) if digit.is_digit(10) =>
            {
                cursor.consume();
                consume_while(cursor, | char | char.is_digit(10));
            }
                    _ => { }
                }

        TokenNumberLiteralKind::Float
            }
            'e' | 'E'
                if match cursor.second() {
        '+' | '-' if cursor.lookup(2).is_digit(10) => true,
                    digit if digit.is_digit(10) => true,
                    _ => false,
                } =>
            {
        cursor.consume();

        match cursor.first() {
            '+' | '-' => {
                cursor.consume();
            }
            _ => { }
                }

        consume_while(cursor, | char | char.is_digit(10));

        TokenNumberLiteralKind::Float
            }
    _ =>
    {
        consume_while(cursor, | char | char.is_digit(10));
        TokenNumberLiteralKind::Integer(TokenIntegerLiteralKind::Decimal)
            }
        }
    }

    //fn consume_single_quoted(cursor: &mut Cursor) -> bool
    //{
    //    // Normal case e.g. 'a'
    //    if cursor.first() != '\\' && cursor.second() == '\'' {
    //        cursor.consume();
    //        cursor.consume();
    //        return true;
    //    }

    //    loop {
    //        match cursor.first() {
    //            '\'' => {
    //                cursor.consume();
    //                return true;
    //            }
    //            '\\' => {
    //                cursor.consume();
    //                cursor.consume();
    //            }
    //            '#' => break,
    //            '\0' => break,
    //            '\n' if cursor.second() != '\'' => break,
    //            _ => {
    //                cursor.consume();
    //            }
    //        }
    //    }

    //    false
    //}

    //fn consume_double_quoted(cursor: &mut Cursor) -> bool
    //{
    //    while let Some(char) = cursor.consume() {
    //        match char {
    //            '"' => return true,
    //            '\\' => {
    //                cursor.consume();
    //            }
    //            _ => { }
    //        }
    //    }

    //    false
    //}
}
