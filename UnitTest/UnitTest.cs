using Lexer.Low;
using Lexer.Low.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class UnitTest
{
    [TestMethod]
    public void CursorTest()
    {
        var cursor = new Cursor("abcde".ToCharArray());
        Assert.AreEqual('a', cursor.First());
        Assert.AreEqual('b', cursor.Second());
        Assert.AreEqual('c', cursor.Lookup(2));
        Assert.AreEqual('\0', cursor.Lookup(5));

        Assert.AreEqual('a', cursor.Consume());
        Assert.AreEqual('b', cursor.Consume());
        Assert.AreEqual('c', cursor.Consume());
        Assert.AreEqual('d', cursor.Consume());
        Assert.AreEqual('e', cursor.Consume());
        Assert.AreEqual(null, cursor.Consume());
    }

    [TestMethod]
    public void TokenLiteralKindInitalizeTest()
    {
        var foo = TokenLiteralKind.DoubleQuotedStr(new TokenStrLiteral(true));
        Assert.AreEqual(true, foo.ToDoubleQuotedStrKind().Terminated);
        var bar = TokenLiteralKind.Number(new TokenNumberLiteral(TokenNumberLiteralKind.Float(), 10213123));
        Assert.AreEqual(10213123, bar.ToNumberKind().SuffixStart);
        Assert.AreEqual(TokenNumberLiteralKindEnum.Float, bar.ToNumberKind().Kind.Enum);
    }

    [TestMethod]
    public void LexTest()
    {
        using var lexer = new Lexer.Low.Lexer("a + b = (c * d) 10000".ToCharArray());
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Plus, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Eq, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.OpenParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Star, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.CloseParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(TokenKindEnum.Literal, lexer.Current.Kind.Enum);
    }
}