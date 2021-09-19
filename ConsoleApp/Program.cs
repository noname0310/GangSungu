using Lexer;
using System;
using System.IO;

const string sourceName = "test.sg";
var input = File.ReadAllText(sourceName);
Console.WriteLine(input!);
HighLexerTest(
            "fn add𪜀(x: i32, y: i32) i32 {" +
            "   return x + y;" +
            "}");

static int GetCollectionCount()
{
    int count = 0;
    for (int i = 0; i <= GC.MaxGeneration; i++)
        count += GC.CollectionCount(i);
    return count;
}

static void HighLexerTest(string content)
{
    var utf32Str = new Utf32String(content);
    using var lexer = new LexEnumerator(new Source(new(new(0), new(utf32Str.Span.Length)), sourceName, SourcePath.Real(sourceName), utf32Str));
    while (lexer.MoveNext())
    {
        var token = lexer.Current;
        var enumStr = token.Kind.Enum.ToString();
        Console.Write(enumStr);
        for (int i = 0; i < 20 - enumStr.Length; i++)
            Console.Write(' ');
        if (token.Kind.Enum == Lexer.Tokens.TokenKindEnum.Id)
            Console.Write(token.Kind.ToId().InternedStr);
        else
            Console.Write(token.Kind);
        Console.WriteLine();
    }
    Console.WriteLine();
    Console.WriteLine($"GC.CollectionCount: { GetCollectionCount() }");
}
