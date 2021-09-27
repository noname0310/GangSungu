namespace GangSungu.Span;

public record class SourcePath
{
    public readonly SourcePathEnum Enum;
    private string _str;

    private SourcePath(SourcePathEnum sourcePathEnum, string str)
    {
        Enum = sourcePathEnum;
        _str = str;
    }
    public ref string ToReal()
    {
        if (Enum != SourcePathEnum.Real)
            throw new InvalidOperationException();
        return ref _str;
    }
    public string ToVirtual()
    {
        if (Enum != SourcePathEnum.Virtual)
            throw new InvalidOperationException();
        return _str;
    }
    public static SourcePath Real(string str) => new(SourcePathEnum.Real, str);
    public static SourcePath Virtual(string str) => new(SourcePathEnum.Virtual, str);
}

public enum SourcePathEnum : short
{
    Real,
    Virtual
}
