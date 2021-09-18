using Lexer;
using System;
using System.IO;

var input = File.ReadAllText("test2.sg");
Console.WriteLine($"GC.CollectionCount: { GetCollectionCount() }");
var lexer = new Lexer.Low.LexEnumerator(input!);
while(lexer.MoveNext())
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

static int GetCollectionCount()
{
    int count = 0;
    for (int i = 0; i <= GC.MaxGeneration; i++)
        count += GC.CollectionCount(i);
    return count;
}
