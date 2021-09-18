using Lexer;
using System;

Console.WriteLine($"GC.CollectionCount: { GetCollectionCount() }");

var input = Console.ReadLine();

foreach (var token in new Lexer.Low.LexEnumerator(input!))
{
    Console.Write(token);
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
