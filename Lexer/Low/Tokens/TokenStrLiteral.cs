namespace Lexer.Low.Tokens;

public readonly record struct TokenStrLiteral
{
    public readonly bool Terminated;
    public TokenStrLiteral(bool terminated) => Terminated = terminated;
}
