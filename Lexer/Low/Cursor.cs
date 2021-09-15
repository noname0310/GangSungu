namespace Lexer.Low;

internal ref struct Cursor
{
    public int LenConsumed => InitialLength - _readOnlySpan.Length;
    public readonly int InitialLength;
    private ReadOnlySpan<char> _readOnlySpan;
    public Cursor(ReadOnlySpan<char> chars)
    {
        InitialLength = chars.Length;
        _readOnlySpan = chars;
    }
    public char First() => Lookup(0); 
    public char Second() => Lookup(1);
    public char Lookup(int offset)
    {
        if (_readOnlySpan.Length <= offset)
            return '\0';
        return _readOnlySpan[offset];
    }
    public char? Consume()
    {
        if (_readOnlySpan.Length <= 0)
            return null;
        char next = _readOnlySpan[0];
        _readOnlySpan = _readOnlySpan[1..];
        return next;
    }
}
