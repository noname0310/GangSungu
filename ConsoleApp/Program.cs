using System;

var input = Console.ReadLine();

using var lexer = new Lexer.Low.LexEnumerator(input!);
while (lexer.MoveNext())
{
    var token = lexer.Current;
    Console.WriteLine(token.Kind.Enum);
}
