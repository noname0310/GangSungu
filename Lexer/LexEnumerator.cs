using Lexer.Tokens;
using System;
using LowLexEnumerator = Lexer.Low.LexEnumerator;
using LowToken = Lexer.Low.Tokens.Token;
using LowTokenKind = Lexer.Low.Tokens.TokenKind;
using LowTokenKindEnum = Lexer.Low.Tokens.TokenKindEnum;
using LowTokenNumberLiteralKindEnum = Lexer.Low.Tokens.TokenNumberLiteralKindEnum;
using LowTokenIntegerLiteralKind = Lexer.Low.Tokens.TokenIntegerLiteralKind;
using LowTokenLiteralKind = Lexer.Low.Tokens.TokenLiteralKind;
using LowTokenLiteralKindEnum = Lexer.Low.Tokens.TokenLiteralKindEnum;
using LowTokenNumberLiteralKind = Lexer.Low.Tokens.TokenNumberLiteralKind;

namespace Lexer;

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
            //if (sliceSpan == "true") //todo make string interner
            //    tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.Bool, new Symbol("true"), null));
            //else if (sliceSpan == "false")
            //    tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.Bool, new Symbol("false"), null));
            //else
                tokenKind = TokenKind.Id(new Symbol(sliceSpan.ToString()));
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
                                new Symbol(str[..number.SuffixStart].ToString()),
                                number.SuffixStart == str.Length ? null : new Symbol(str[number.SuffixStart..].ToString())
                            ));
                    }
                    break;
                case LowTokenLiteralKindEnum.SingleQuotedStr:
                    tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.SingleQuotedStr, new Symbol(str.ToString()), null));
                    break;
                case LowTokenLiteralKindEnum.DoubleQuotedStr:
                    tokenKind = TokenKind.Literal(new TokenLiteral(TokenLiteralKind.DoubleQuotedStr, new Symbol(str.ToString()), null));
                    break;
            }
        }
        if (tokenKind == null)
            return null;
        return new Token(tokenKind.Value, span);
    }

    public static void CheckLowToken(in LowToken lowToken, Span span, in Source source) {
        //switch (lowToken.Kind.Enum)
        //{
        //    case LowTokenKindEnum.Unknown:
        //        Diagnostic::push_new(Diagnostic::new(
        //            Level::Error,
        //            format!("unknown token '{}'", source.slice(span)),
        //            MultiSpan::with_spans(vec![(
        //                format!("'{}' is not allowed", source.slice(span)),
        //                Some(span),
        //            )]),
        //        )),
        //        break;
        //    case LowTokenKindEnum.Whitespace: break;
        //    case LowTokenKindEnum.Comment: break;
        //    case LowTokenKindEnum.OpenParen:  break;
        //    case LowTokenKindEnum.CloseParen: break;
        //    case LowTokenKindEnum.OpenBrace: break;
        //    case LowTokenKindEnum.CloseBrace: break;
        //    case LowTokenKindEnum.OpenBracket: break;
        //    case LowTokenKindEnum.CloseBracket: break;
        //    case LowTokenKindEnum.Dot: break;
        //    case LowTokenKindEnum.Comma: break;
        //    case LowTokenKindEnum.Colon: break;
        //    case LowTokenKindEnum.Semicolon: break;
        //    case LowTokenKindEnum.Eq: break;
        //    case LowTokenKindEnum.Bang: break;
        //    case LowTokenKindEnum.Lt:  break;
        //    case LowTokenKindEnum.Gt: break;
        //    case LowTokenKindEnum.Plus: break;
        //    case LowTokenKindEnum.Minus: break;
        //    case LowTokenKindEnum.Star: break;
        //    case LowTokenKindEnum.Slash: break;
        //    case LowTokenKindEnum.Percent: break;
        //    case LowTokenKindEnum.Or: break;
        //    case LowTokenKindEnum.And: break;
        //    case LowTokenKindEnum.Caret: break;
        //    case LowTokenKindEnum.Tilde: break;
        //    case LowTokenKindEnum.Id: break;
        //    case LowTokenKindEnum.Literal:
        //        var literal = lowToken.Kind.ToLiteral();
        //        switch (literal.Enum)
        //        {
        //            case LowTokenLiteralKindEnum.Number:
        //                if number.suffix_start() != token.len() {
        //                    let suffix = &source.slice(span)[number.suffix_start()..];

        //                    // TODO: Perform value overflow check.

        //                    match number.kind() {
        //                        LowTokenNumberLiteralKind::Integer(integer) => match suffix {
        //                            suffix if is_integer_suffix(suffix) => {}
        //                            suffix if is_float_suffix(suffix) => {
        //                                if !integer.is_decimal() {
        //                                    Diagnostic::push_new(Diagnostic::new(
        //                                        Level::Error,
        //                                        format!("invalid use of float suffix '{}'", suffix),
        //                                        MultiSpan::with_spans(vec![
        //                                            (
        //                                                format!("float suffix '{}' is not allowed for non-decimal integer literals", suffix),
        //                                                Some(Span::new(
        //                                                    span.low().offset(number.suffix_start() as _),
        //                                                    span.high(),
        //                                                )),
        //                                            ),
        //                                            (format!("consider use integer suffix or remove it"), None),
        //                                        ]),
        //                                    ));
        //                                }
        //                            }
        //                            suffix => {
        //                                Diagnostic::push_new(Diagnostic::new(
        //                                    Level::Error,
        //                                    format!("invalid suffix '{}'", suffix),
        //                                    MultiSpan::with_spans(vec![
        //                                        (
        //                                            format!("'{}' is not valid suffix", suffix),
        //                                            Some(Span::new(
        //                                                span.low().offset(number.suffix_start() as _),
        //                                                span.high(),
        //                                            )),
        //                                        ),
        //                                        (format!("consider use integer suffix or remove it"), None),
        //                                    ]),
        //                                ));
        //                            }
        //                        },
        //                        LowTokenNumberLiteralKind::Float => match suffix {
        //                            suffix if is_float_suffix(suffix) => {}
        //                            suffix if is_integer_suffix(suffix) => {
        //                                Diagnostic::push_new(Diagnostic::new(
        //                                    Level::Error,
        //                                    format!("invalid use of integer suffix '{}'", suffix),
        //                                    MultiSpan::with_spans(vec![
        //                                        (
        //                                            format!("integer suffix '{}' is not allowed for float literals", suffix),
        //                                            Some(Span::new(
        //                                                span.low().offset(number.suffix_start() as _),
        //                                                span.high(),
        //                                            )),
        //                                        ),
        //                                        (format!("consider use 'f64' or remove it"), None),
        //                                    ]),
        //                                ));
        //                            }
        //                            suffix => {
        //                                Diagnostic::push_new(Diagnostic::new(
        //                                    Level::Error,
        //                                    format!("invalid suffix '{}'", suffix),
        //                                    MultiSpan::with_spans(vec![
        //                                        (
        //                                            format!("'{}' is not valid suffix", suffix),
        //                                            Some(Span::new(
        //                                                span.low().offset(number.suffix_start() as _),
        //                                                span.high(),
        //                                            )),
        //                                        ),
        //                                        (format!("consider use 'f64' or remove it"), None),
        //                                    ]),
        //                                ));
        //                            }
        //                        },
        //                    }
        //                }
        //                break;
        //            case LowTokenLiteralKindEnum.SingleQuotedStr:
        //                {
        //                    var str = literal.ToSingleQuotedStr();
        //                    // TODO: Detect long-length literals and emit diagnostics for it.
        //                    if (!str.Terminated) {
        //                        Diagnostic::push_new(Diagnostic::new(
        //                            Level::Error,
        //                            format!("single quoted literal is not closed"),
        //                            MultiSpan::with_spans(vec![
        //                                (format!("' is missing"), Some(span)),
        //                                (format!("add ' at the end of the literal"), None),
    

        //                            ]),
    

        //                        ));
        //                    }
        //                }
        //                break;
        //            case LowTokenLiteralKindEnum.DoubleQuotedStr:
        //                var str = literal.ToDoubleQuotedStr();
        //                if !str.terminated() {
        //                    Diagnostic::push_new(Diagnostic::new(
        //                        Level::Error,
        //                        format!("double quoted literal is not closed"),
        //                        MultiSpan::with_spans(vec![
        //                            (format!("\" is missing"), Some(span)),
        //                            (format!("add \" at the end of the literal"), None),
        
        //                        ]),
        
        //                    ));
        //                }
        //                break;
        //        }
        //        break;
        //}
    }

    private static bool IsIntegerSuffix(ReadOnlySpan<char> suffix)
    {
        if (suffix == "byte"
            || suffix == "char"
            || suffix == "i64"
            || suffix == "u64"
            || suffix == "usize")
            return true;
        return false;
    }

    private static bool IsFloatSuffix(ReadOnlySpan<char> suffix)
    {
        if (suffix == "f64")
            return true;
        return false;
    }
}
