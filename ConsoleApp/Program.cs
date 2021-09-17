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
    Console.WriteLine(token);
}
