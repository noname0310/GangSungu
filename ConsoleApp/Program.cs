using Lexer;
using System;

var pos = new Pos(1000);
Console.WriteLine(pos);

var span = new Span(pos, pos);
Console.WriteLine(span);

var input = Console.ReadLine();

using var lexer = new Lexer.Low.LexEnumerator(input!);
while (lexer.MoveNext())
{
    var token = lexer.Current;
    Console.Write(token);
    if(token.Kind.Enum == Lexer.Low.Tokens.TokenKindEnum.Literal)
    {
        Console.Write(' ');
        Console.Write(token.Kind.ToLiteral());
    }
    Console.WriteLine();
}
