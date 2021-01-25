using System;
using System.Collections.Generic;
using SyntaxAnalyzer.Lexers;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    internal static class EnumeratorExtensions
    {
        public static IToken? GetNextToken(this IEnumerator<IToken> enumerator)
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            return null;
        }

        public static IToken? AssertToken(this IEnumerator<IToken> enumerator, TokenType tokenType)
        {
            var token = enumerator.Current;
            if (token == null) throw new InvalidOperationException($"Expected token '{tokenType}', but no token was retrieved");

            if (token.Type == tokenType)
            {
                return enumerator.GetNextToken();
            }

            throw new InvalidOperationException($"Expected token '{tokenType} but received '{token.Value}' ('{token.Type}')");
        }
    }
}