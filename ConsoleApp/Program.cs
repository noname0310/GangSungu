using Lexer;
using System;
using System.IO;

const string sourceName = "test.sg";
var input = File.ReadAllText(sourceName);
Console.WriteLine(input!);
HighLexerTest(input!);

static int GetCollectionCount()
{
    int count = 0;
    for (int i = 0; i <= GC.MaxGeneration; i++)
        count += GC.CollectionCount(i);
    return count;
}

static void LowLexerTest(string input)
{
    Console.WriteLine($"GC.CollectionCount: { GetCollectionCount() }");
    var lexer = new Lexer.Low.LexEnumerator(input);
    while (lexer.MoveNext())
    {
        var token = lexer.Current;
        Console.WriteLine(token.Kind.Enum);
        if (token.Kind.Enum == Lexer.Low.Tokens.TokenKindEnum.Literal)
        {
            Console.Write(' ');
            Console.Write(token.Kind.ToLiteral());
        }
        Console.WriteLine();
    }

    Console.WriteLine($"GC.CollectionCount: { GetCollectionCount() }");
}

static void HighLexerTest(string content)
{
    var lexer = new LexEnumerator(new Source(new(new(0), new(content.Length)), sourceName, SourcePath.Real(sourceName), content));
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
