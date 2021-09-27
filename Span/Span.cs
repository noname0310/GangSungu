namespace GangSungu.Span;

public readonly record struct Span
{
    public readonly Pos Low;
    public readonly Pos High;
    public int Length => High - Low;
    public Span(Pos low, Pos high)
    {
        Low = low;
        High = high;
    }
    public bool Contains(Span other) => Low <= other.Low && High >= other.High;
    public Span To(Span to) => new(Low, to.High);
    public Span Merge(Span other) => new(Low < other.Low ? Low : other.Low, High > other.High ? High : other.High);
}
