using System;

namespace Lexer;

public readonly record struct Pos : IEquatable<Pos>, IComparable<Pos>
{
    public readonly int Value;
    public Pos(int pos) => Value = pos;
    public Pos Offset(int offset) => new(Value + offset);
    public bool Equals(Pos other) => Value == other.Value;
    public int CompareTo(Pos other) => Value.CompareTo(other.Value);
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator <=(Pos lhs, Pos rhs) => lhs.Value <= rhs.Value;
    public static bool operator >=(Pos lhs, Pos rhs) => lhs.Value >= rhs.Value;
    public static bool operator <(Pos lhs, Pos rhs) => lhs.Value < rhs.Value;
    public static bool operator >(Pos lhs, Pos rhs) => lhs.Value > rhs.Value;
    public static int operator +(Pos lhs, Pos rhs) => lhs.Value + rhs.Value;
    public static int operator -(Pos lhs, Pos rhs) => lhs.Value - rhs.Value;
}
