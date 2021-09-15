namespace Lexer.Low.Tokens;

public readonly struct Token
{
    public readonly TokenKind Kind;
    public readonly int Length;
    private readonly TokenLiteral _tokenLiteral;

    private Token(TokenKind kind, int length, TokenLiteral tokenLiteral = new())
    {
        Kind = kind;
        Length = length;
        _tokenLiteral = tokenLiteral;
    }

    public TokenLiteral ToTokenLiteral()
    {
        if (Kind != TokenKind.Literal)
            throw new InvalidOperationException();
        return _tokenLiteral;
    }

    public static Token UnknownKind(int length) => new(TokenKind.Unknown, length);
    public static Token WhitespaceKind(int length) => new(TokenKind.Whitespace, length);
    public static Token CommentKind(int length) => new(TokenKind.Comment, length);
    public static Token OpenParenKind(int length) => new(TokenKind.OpenParen, length);
    public static Token CloseParenKind(int length) => new(TokenKind.CloseParen, length);
    public static Token OpenBraceKind(int length) => new(TokenKind.OpenBrace, length);
    public static Token CloseBraceKind(int length) => new(TokenKind.CloseBrace, length);
    public static Token OpenBracketKind(int length) => new(TokenKind.OpenBracket, length);
    public static Token CloseBracketKind(int length) => new(TokenKind.CloseBracket, length);
    public static Token DotKind(int length) => new(TokenKind.Dot, length);
    public static Token CommaKind(int length) => new(TokenKind.Comma, length);
    public static Token ColonKind(int length) => new(TokenKind.Colon, length);
    public static Token SemicolonKind(int length) => new(TokenKind.Semicolon, length);
    public static Token EqKind(int length) => new(TokenKind.Eq, length);
    public static Token BangKind(int length) => new(TokenKind.Bang, length);
    public static Token LtKind(int length) => new(TokenKind.Lt, length);
    public static Token GtKind(int length) => new(TokenKind.Gt, length);
    public static Token PlusKind(int length) => new(TokenKind.Plus, length);
    public static Token MinusKind(int length) => new(TokenKind.Minus, length);
    public static Token StarKind(int length) => new(TokenKind.Star, length);
    public static Token SlashKind(int length) => new(TokenKind.Slash, length);
    public static Token PercentKind(int length) => new(TokenKind.Percent, length);
    public static Token OrKind(int length) => new(TokenKind.Or, length);
    public static Token AndKind(int length) => new(TokenKind.And, length);
    public static Token CaretKind(int length) => new(TokenKind.Caret, length);
    public static Token TildeKind(int length) => new(TokenKind.Tilde, length);
    public static Token IdKind(int length) => new(TokenKind.Id, length);
    public static Token LiteralKind(int length, TokenLiteral tokenLiteral) => new(TokenKind.Literal, length, tokenLiteral);
}
