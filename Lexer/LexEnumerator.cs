﻿using Lexer.Tokens;
using System.Collections;
using System.Collections.Generic;

namespace Lexer;

public struct LexEnumerator : IEnumerator<Token>
{
    public Token Current => throw new System.NotImplementedException();

    object IEnumerator.Current => throw new System.NotImplementedException();

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public bool MoveNext()
    {
        throw new System.NotImplementedException();
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}
