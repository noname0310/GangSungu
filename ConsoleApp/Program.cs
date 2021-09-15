using Lexer.Low.Tokens;
using System.Runtime.InteropServices;

var input = "3 3 3 3 3";

using var lexer = new Lexer.Low.Lexer(input!.ToCharArray());
while (lexer.MoveNext())
{
    var token = lexer.Current;
    Console.WriteLine(token.Kind.Enum);
}