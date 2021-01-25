using System;

namespace SyntaxAnalyzer.Lexers
{
    public interface IToken
    {
        TokenType Type { get; }
        string Value { get; }
    }

    public record Token (string Value, TokenType Type) : IToken
    {
        public Token(ReadOnlySpan<char> span, TokenType type) : this(new string(span), type)
        {
        }

        public Token(char value, TokenType type) : this(value.ToString(), type)
        {
        }
    }
}