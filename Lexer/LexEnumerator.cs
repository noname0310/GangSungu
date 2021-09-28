using GangSungu.Lexer.Tokens;
using System;
using LowLexEnumerator = GangSungu.Lexer.Low.LexEnumerator;
using LowToken = GangSungu.Lexer.Low.Tokens.Token;
using LowTokenKindEnum = GangSungu.Lexer.Low.Tokens.TokenKindEnum;
using LowTokenNumberLiteralKindEnum = GangSungu.Lexer.Low.Tokens.TokenNumberLiteralKindEnum;
using LowTokenIntegerLiteralKind = GangSungu.Lexer.Low.Tokens.TokenIntegerLiteralKind;
using LowTokenLiteralKindEnum = GangSungu.Lexer.Low.Tokens.TokenLiteralKindEnum;

namespace GangSungu.Lexer;

using GangSungu.Span;
using GangSungu.Diagnostic;
using Char32 = UInt32;

public ref struct LexEnumerator
{
    private ref struct UngluedTokenEnumerator
    {
        public Token Current { get; private set; }
        private readonly Source _source;
        private Pos _low;
        private LowLexEnumerator _lowLexer;
        public UngluedTokenEnumerator(Source source)
        {
            _source = source;
            _low = source.Span.Low;
            _lowLexer = new LowLexEnumerator(source.Content);
            Current = default;
        }
        public bool MoveNext()
        {
            for (; ; )
            {
                if (_lowLexer.MoveNext() == false) return false;
                var lowtoken = _lowLexer.Current;
                var token = Convert(lowtoken, _low, _source);
                _low = _low.Offset(lowtoken.Length);
                if (token.HasValue)
                {
                    Current = token.Value;
                    return true;
                }
            }
        }
        public void Reset()
        {
            _low = _source.Span.Low;
            _lowLexer.Reset();
            Current = default;
        }
    }

    public Token Current { get; private set; }
    private UngluedTokenEnumerator _ungluedTokenEnumerator;
    private Token? _current;
    private Token? _next;
    public LexEnumerator(Source source)
    {
        _ungluedTokenEnumerator = new(source);

        if (_ungluedTokenEnumerator.MoveNext())
            _current = _ungluedTokenEnumerator.Current;
        else
            _current = null;
        
        if (_ungluedTokenEnumerator.MoveNext())
            _next = _ungluedTokenEnumerator.Current;
        else
            _next = null;
        
        Current = default;
    }
    public bool MoveNext()
    {
        if (!_current.HasValue)
            return false;

        var token = _current.Value;

        for (; ; )
        {
            if (!_next.HasValue)
                break;
            var glued = token.Glue(_next.Value);
            if (glued.HasValue)
            {
                if (_ungluedTokenEnumerator.MoveNext())
                    _next = _ungluedTokenEnumerator.Current;
                else
                    _next = null;
                token = glued.Value;
            }
            else
                break;
        }

        _current = _next; 
        if (_ungluedTokenEnumerator.MoveNext())
            _next = _ungluedTokenEnumerator.Current;
        else
            _next = null;
        Current = token;
        return true;
    }
    public void Reset()
    {
        _ungluedTokenEnumerator.Reset();
        if (_ungluedTokenEnumerator.MoveNext())
            _current = _ungluedTokenEnumerator.Current;
        else
            _current = null;

        if (_ungluedTokenEnumerator.MoveNext())
            _next = _ungluedTokenEnumerator.Current;
        else
            _next = null;

        Current = default;
    }
    // TODO: Add diagnostics for error reporting.
    private static Token? Convert(LowToken lowToken, Pos low, in Source source)
    {
        var span = new Span(low, low.Offset(lowToken.Length));

        CheckLowToken(in lowToken, span, in source);

        TokenKind? tokenKind = lowToken.Kind.Enum switch
        {
            LowTokenKindEnum.Unknown
            or LowTokenKindEnum.Whitespace
            or LowTokenKindEnum.Comment => null,
            LowTokenKindEnum.OpenParen => TokenKind.OpenParen(),
            LowTokenKindEnum.CloseParen => TokenKind.CloseParen(),
            LowTokenKindEnum.OpenBrace => TokenKind.OpenBrace(),
            LowTokenKindEnum.CloseBrace => TokenKind.CloseBrace(),
            LowTokenKindEnum.OpenBracket => TokenKind.OpenBracket(),
            LowTokenKindEnum.CloseBracket => TokenKind.CloseBracket(),
            LowTokenKindEnum.Dot => TokenKind.Dot(),
            LowTokenKindEnum.Comma => TokenKind.Comma(),
            LowTokenKindEnum.Colon => TokenKind.Colon(),
            LowTokenKindEnum.Semicolon => TokenKind.Semicolon(),
            LowTokenKindEnum.Eq => TokenKind.Assign(),
            LowTokenKindEnum.Bang => TokenKind.LogNot(),
            LowTokenKindEnum.Lt => TokenKind.Lt(),
            LowTokenKindEnum.Gt => TokenKind.Gt(),
            LowTokenKindEnum.Plus => TokenKind.Add(),
            LowTokenKindEnum.Minus => TokenKind.Sub(),
            LowTokenKindEnum.Star => TokenKind.Mul(),
            LowTokenKindEnum.Slash => TokenKind.Div(),
            LowTokenKindEnum.Percent => TokenKind.Mod(),
            LowTokenKindEnum.Or => TokenKind.BitOr(),
            LowTokenKindEnum.And => TokenKind.BitAnd(),
            LowTokenKindEnum.Caret => TokenKind.BitXor(),
            LowTokenKindEnum.Tilde => TokenKind.BitNot(),
            _ => null
        };
        if (lowToken.Kind.Enum == LowTokenKindEnum.Id) 
        {
            var sliceSpan = source.Slice(span);
            if (sliceSpan.Compare("true"))
                tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.Bool, new Symbol("true"), null));
            else if (sliceSpan.Compare("false"))
                tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.Bool, new Symbol("false"), null));
            else
                tokenKind = TokenKind.Id(new Symbol(sliceSpan.ToUtf16String()));
        }
        else if (lowToken.Kind.Enum == LowTokenKindEnum.Literal)
        {
            var literal = lowToken.Kind.ToLiteral();
            var str = source.Slice(span);

            switch (literal.Enum)
            {
                case LowTokenLiteralKindEnum.Number:
                    {
                        var number = literal.ToNumber();
                        tokenKind = TokenKind.Literal(
                            new TokenLiteral(number.Kind.Enum switch
                            {
                                LowTokenNumberLiteralKindEnum.Integer => number.Kind.ToInteger() switch
                                {
                                    LowTokenIntegerLiteralKind.Binary => TokenLiteralKind.IntegerBinary,
                                    LowTokenIntegerLiteralKind.Octal => TokenLiteralKind.IntegerOctal,
                                    LowTokenIntegerLiteralKind.Hexadecimal => TokenLiteralKind.IntegerHexadecimal,
                                    LowTokenIntegerLiteralKind.Decimal => TokenLiteralKind.IntegerDecimal,
                                    _ => throw new InvalidOperationException(),
                                },
                                LowTokenNumberLiteralKindEnum.Float => TokenLiteralKind.Float,
                                _ => throw new InvalidOperationException(),
                            },
                                new Symbol(str[..number.SuffixStart].ToUtf16String()),
                                number.SuffixStart == str.Length ? null : new Symbol(str[number.SuffixStart..].ToString())
                            ));
                    }
                    break;
                case LowTokenLiteralKindEnum.SingleQuotedStr:
                    tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.SingleQuotedStr, new Symbol(str.ToUtf16String()), null));
                    break;
                case LowTokenLiteralKindEnum.DoubleQuotedStr:
                    tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.DoubleQuotedStr, new Symbol(str.ToUtf16String()), null));
                    break;
            }
        }
        if (tokenKind == null)
            return null;
        return new Token(tokenKind.Value, span);
    }
    public static void CheckLowToken(in LowToken lowToken, Span span, in Source source)
    {
        switch (lowToken.Kind.Enum)
        {
            case LowTokenKindEnum.Unknown:
                new Diagnostic(
                    Level.Error, $"unknown token '{source.Slice(span).ToUtf16String()}'",
                    new MultiSpan(new() { new("'{}' is not allowed", span) }))
                    .Register();
                break;
            case LowTokenKindEnum.Whitespace: break;
            case LowTokenKindEnum.Comment: break;
            case LowTokenKindEnum.OpenParen: break;
            case LowTokenKindEnum.CloseParen: break;
            case LowTokenKindEnum.OpenBrace: break;
            case LowTokenKindEnum.CloseBrace: break;
            case LowTokenKindEnum.OpenBracket: break;
            case LowTokenKindEnum.CloseBracket: break;
            case LowTokenKindEnum.Dot: break;
            case LowTokenKindEnum.Comma: break;
            case LowTokenKindEnum.Colon: break;
            case LowTokenKindEnum.Semicolon: break;
            case LowTokenKindEnum.Eq: break;
            case LowTokenKindEnum.Bang: break;
            case LowTokenKindEnum.Lt: break;
            case LowTokenKindEnum.Gt: break;
            case LowTokenKindEnum.Plus: break;
            case LowTokenKindEnum.Minus: break;
            case LowTokenKindEnum.Star: break;
            case LowTokenKindEnum.Slash: break;
            case LowTokenKindEnum.Percent: break;
            case LowTokenKindEnum.Or: break;
            case LowTokenKindEnum.And: break;
            case LowTokenKindEnum.Caret: break;
            case LowTokenKindEnum.Tilde: break;
            case LowTokenKindEnum.Id: break;
            case LowTokenKindEnum.Literal:
                var literal = lowToken.Kind.ToLiteral();
                switch (literal.Enum)
                {
                    case LowTokenLiteralKindEnum.Number:
                        {
                            var number = literal.ToNumber();
                            if (number.SuffixStart != lowToken.Length)
                            {
                                var suffix = source.Slice(span)[number.SuffixStart..];

                                // TODO: Perform value overflow check.

                                switch (number.Kind.Enum)
                                {
                                    case LowTokenNumberLiteralKindEnum.Integer:
                                        {
                                            var integer = number.Kind.ToInteger();
                                            if (IsIntegerSuffix(suffix))
                                                break;
                                            else if (IsFloatSuffix(suffix))
                                            {
                                                if (integer != LowTokenIntegerLiteralKind.Decimal)
                                                {
                                                    new Diagnostic(
                                                        Level.Error,
                                                        $"invalid use of float suffix '{suffix.ToUtf16String()}'",
                                                        new MultiSpan(new()
                                                        {
                                                            new(
                                                                $"float suffix '{suffix.ToUtf16String()}' is not allowed for non-decimal integer literals",
                                                                new(span.Low.Offset(number.SuffixStart), span.High)),
                                                            new("consider use integer suffix or remove it", null)
                                                        })).Register();
                                                }
                                            }
                                            else
                                            {
                                                new Diagnostic(
                                                    Level.Error,
                                                    $"invalid suffix '{suffix.ToUtf16String()}'",
                                                    new MultiSpan(new()
                                                    {
                                                        new(
                                                            $"'{suffix.ToUtf16String()}' is not valid suffix",
                                                            new(span.Low.Offset(number.SuffixStart), span.High)),
                                                        new("consider use integer suffix or remove it", null)
                                                    })).Register();
                                            }
                                        }
                                        break;
                                    case LowTokenNumberLiteralKindEnum.Float:
                                        {
                                            if (IsFloatSuffix(suffix))
                                                break;
                                            else if (IsIntegerSuffix(suffix))
                                            {
                                                new Diagnostic(
                                                    Level.Error,
                                                    $"invalid use of integer suffix '{suffix.ToUtf16String()}'",
                                                    new MultiSpan(new()
                                                    {
                                                        new(
                                                            $"integer suffix '{suffix.ToUtf16String()}' is not allowed for float literals",
                                                            new(span.Low.Offset(number.SuffixStart), span.High)),
                                                        new("consider use 'f64' or remove it", null)
                                                    })).Register();
                                            }
                                            else
                                            {
                                                new Diagnostic(
                                                    Level.Error,
                                                    $"invalid suffix '{suffix.ToUtf16String()}'",
                                                    new MultiSpan(new()
                                                    {
                                                        new(
                                                            $"'{suffix.ToUtf16String()}' is not valid suffix",
                                                            new(span.Low.Offset(number.SuffixStart), span.High)),
                                                        new("consider use 'f64' suffix or remove it", null)
                                                    })).Register();
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                    case LowTokenLiteralKindEnum.SingleQuotedStr:
                        {
                            var str = literal.ToSingleQuotedStr();
                            if (!str.Terminated)
                            {
                                new Diagnostic(
                                    Level.Error,
                                    "single quoted literal is not closed",
                                    new MultiSpan(new()
                                    {
                                        new("' is missing", span),
                                        new("add ' at the end of the literal", null)
                                    })).Register();
                            }
                        }
                        break;
                    case LowTokenLiteralKindEnum.DoubleQuotedStr:
                        {
                            var str = literal.ToDoubleQuotedStr();
                            if (!str.Terminated)
                            {
                                new Diagnostic(
                                    Level.Error,
                                    "double quoted literal is not closed",
                                    new MultiSpan(new()
                                    {
                                        new("\" is missing", span),
                                        new("add \" at the end of the literal", null)
                                    })).Register();
                            }
                        }
                        break;
                }
                break;
        }
    }
    private static bool IsIntegerSuffix(ReadOnlySpan<Char32> suffix)
    {
        if (suffix.Compare("byte")
            || suffix.Compare("char")
            || suffix.Compare("i64")
            || suffix.Compare("u64")
            || suffix.Compare("usize"))
            return true;
        return false;
    }

    private static bool IsFloatSuffix(ReadOnlySpan<Char32> suffix)
    {
        if (suffix.Compare("f64"))
            return true;
        return false;
    }
}
