using Lexer;
using System;
using System.IO;
using System.Diagnostics;

const string sourceName = "test2.sg";
var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), sourceName));
HighLexerTest(input!);

static int GetCollectionCount()
{
    int count = 0;
    for (int i = 0; i <= GC.MaxGeneration; i++)
        count += GC.CollectionCount(i);
    return count;
}

static void HighLexerTest(string content)
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    using var utf32Str = new Utf32String(content);
    var lexer = new LexEnumerator(new Source(new(new(0), new(utf32Str.Span.Length)), sourceName, SourcePath.Real(sourceName), utf32Str.Span));
    while (lexer.MoveNext())
    {
        var token = lexer.Current;
        var enumStr = token.Kind.Enum.ToString();
        Console.Write(enumStr);
        for (int i = 0; i < 20 - enumStr.Length; i++)
            Console.Write(' ');
        if (token.Kind.Enum == Lexer.Tokens.TokenKindEnum.Id)
            Console.Write(token.Kind.ToId().InternedStr);
        else if (token.Kind.Enum == Lexer.Tokens.TokenKindEnum.Literal)
            Console.Write(token.Kind.ToLiteral().Str.InternedStr);
        else
            Console.Write(token.Kind);
        Console.WriteLine();
    }
    stopwatch.Stop();
    Console.WriteLine();
    Console.WriteLine($"GC.CollectionCount: { GetCollectionCount() }, ElapsedMilliseconds: {stopwatch.ElapsedMilliseconds}");
}
