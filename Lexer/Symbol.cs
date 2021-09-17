using System;

namespace Lexer;

public struct Symbol : IEquatable<Symbol>
{
    public readonly string InternedStr;
    public Symbol(string str) => InternedStr = string.Intern(str);
    public bool Equals(Symbol other) => ReferenceEquals(InternedStr, other.InternedStr);
    public override bool Equals(object? obj) => obj is Symbol symbol && Equals(symbol);
    public override int GetHashCode() => Str.GetHashCode();
    public override string? ToString() => $"Symbol {{ InternedStr = {InternedStr} }}";
    public static bool operator ==(Symbol lhs, Symbol rhs) => ReferenceEquals(lhs.InternedStr, rhs.InternedStr);
    public static bool operator !=(Symbol lhs, Symbol rhs) => !ReferenceEquals(lhs.InternedStr, rhs.InternedStr);

    public static readonly Symbol Empty = new("");
    public static readonly Symbol Main = new("main");
    public static readonly Symbol Bool = new("bool");
    public static readonly Symbol Byte = new("byte");
    public static readonly Symbol Char = new("char");
    public static readonly Symbol I64 = new("i64");
    public static readonly Symbol U64 = new("u64");
    public static readonly Symbol Isize = new("isize");
    public static readonly Symbol Usize = new("usize");
    public static readonly Symbol F64 = new("f64");
    public static readonly Symbol Str = new("str");
    public static readonly Symbol Cptr = new("cptr");
    public static readonly Symbol Mptr = new("mptr");
    public static readonly Symbol As = new("as");
    public static readonly Symbol Use = new("use");
    public static readonly Symbol Extern = new("extern");
    public static readonly Symbol Pub = new("pub");
    public static readonly Symbol Fn = new("fn");
    public static readonly Symbol Break = new("break");
    public static readonly Symbol Continue = new("continue");
    public static readonly Symbol Return = new("return");
    public static readonly Symbol If = new("if");
    public static readonly Symbol Else = new("else");
    public static readonly Symbol For = new("for");
    public static readonly Symbol In = new("in");
    public static readonly Symbol Let = new("let");
    public static readonly Symbol Struct = new("struct");
}
