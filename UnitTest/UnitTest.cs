using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace UnitTest;

[TestClass]
public class UnitTest
{
    [TestMethod]
    public void CursorTest()
    {
        var cursor = new Lexer.Low.Cursor(Encoding.UTF32.GetBytes("abcde"));
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
        var foo = Lexer.Low.Tokens.TokenLiteralKind.DoubleQuotedStr(new Lexer.Low.Tokens.TokenStrLiteral(true));
        Assert.AreEqual(true, foo.ToDoubleQuotedStr().Terminated);
        var bar = Lexer.Low.Tokens.TokenLiteralKind.Number(new Lexer.Low.Tokens.TokenNumberLiteral(Lexer.Low.Tokens.TokenNumberLiteralKind.Float(), 10213123));
        Assert.AreEqual(10213123, bar.ToNumber().SuffixStart);
        Assert.AreEqual(Lexer.Low.Tokens.TokenNumberLiteralKindEnum.Float, bar.ToNumber().Kind.Enum);
    }

    [TestMethod]
    public void LowLexTest()
    {
        using var lexer = new Lexer.Low.LexEnumerator("a + b = (c * d) 10000");
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Plus, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Eq, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.OpenParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Star, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.CloseParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Literal, lexer.Current.Kind.Enum);
    }

    [TestMethod]
    public void LowLexTest2()
    {
        using var lexer = new Lexer.Low.LexEnumerator("히히히𪜀 + 헤헤헤 = (호호호 * 하하핳) 10000");
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Plus, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Eq, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.OpenParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Star, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.CloseParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Whitespace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Low.Tokens.TokenKindEnum.Literal, lexer.Current.Kind.Enum);
    }

    [TestMethod]
    public void LexTest()
    {
        var source =
            "fn add(x: i32, y: i32) i32 {" +
            "   return x + y;" +
            "}";
        using var lexer = new Lexer.LexEnumerator(new(new(new(0), new(source.Length)), "test", Lexer.SourcePath.Real("test"), source));
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.OpenParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Colon, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Comma, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Colon, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.CloseParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.OpenBrace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Add, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Semicolon, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.CloseBrace, lexer.Current.Kind.Enum);
    }

    [TestMethod]
    public void LexTest2()
    {
        var source =
            "fn add𪜀(x: i32, y: i32) i32 {" +
            "   return x + y;" +
            "}";
        using var lexer = new Lexer.LexEnumerator(new(new(new(0), new(source.Length)), "test", Lexer.SourcePath.Real("test"), source));
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.OpenParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Colon, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Comma, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Colon, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.CloseParen, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.OpenBrace, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Add, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Id, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.Semicolon, lexer.Current.Kind.Enum);
        lexer.MoveNext();
        Assert.AreEqual(Lexer.Tokens.TokenKindEnum.CloseBrace, lexer.Current.Kind.Enum);
    }
}
