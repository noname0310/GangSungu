using Lexer.Low;

var cursor = new Cursor("abcde".ToCharArray());
Console.WriteLine('a' == cursor.First());
Console.WriteLine('b' == cursor.Second());

Console.WriteLine('a' == cursor.Consume());
Console.WriteLine('b' == cursor.Consume());
Console.WriteLine('c' == cursor.Consume());
Console.WriteLine('d' == cursor.Consume());
Console.WriteLine('e' == cursor.Consume());
Console.WriteLine(null == cursor.Consume());