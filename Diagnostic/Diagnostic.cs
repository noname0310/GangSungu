namespace Diagnostic;

public struct Diagnostic
{
    public readonly Level Level;
    public readonly string Message;
    public readonly MultiSpan MultiSpan;
    
    public Diagnostic(Level level, string message, MultiSpan span)
    {
        Level = level;
        Message = message;
        MultiSpan = span;
    }
    public void Register()
    {

    }
}
