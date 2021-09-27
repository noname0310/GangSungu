namespace GangSungu.Span;

public readonly record struct LineCol
{
    public readonly int Line;
    public readonly int Col;

    public LineCol(int line, int col)
    {
        Line = line;
        Col = col;
    }
}
