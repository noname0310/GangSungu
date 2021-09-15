using Lexer.Low;
using Lexer.Low.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;

[TestClass]
public class UnitTest1
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
}