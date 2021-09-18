using System.Collections.Generic;

namespace Lexer;
public static class EnumeratorExtension
{
    public static IEnumerator<Tokens.Token> GetEnumerator(this IEnumerator<Tokens.Token> enumerator) => enumerator;
    public static IEnumerator<Low.Tokens.Token> GetEnumerator(this IEnumerator<Low.Tokens.Token> enumerator) => enumerator;
}
