using System;
using System.Collections.Generic;
using System.Linq;
using SyntaxAnalyzer.Lexers;

namespace SyntaxAnalyzer
{
    public enum TokenType
    {
        Number,
        Identifier,
        SignPlus,
        SignMinus,
        SignMultiply,
        SignDivide,
        KeywordFor,
        KeywordWhile,
        BraceCurlyOpen,
        BraceCurlyClose,
        ParenthesesOpen,
        ParenthesesClose,
        Not,
        Equals,
        EqualsEquals,
        LessEquals,
        NotEquals,
        GreaterEquals,
        Less,
        Greater,
        KeywordIf,
        KeywordElse,
        EOF
    }

    public class Lexer
    {
        private static readonly Dictionary<char, TokenType> SpecialCharacters = new()
        {
            {'+', TokenType.SignPlus},
            {'-', TokenType.SignMinus},
            {'/', TokenType.SignDivide},
            {'*', TokenType.SignMultiply},
            {'=', TokenType.Equals},
            {'<', TokenType.Less},
            {'>', TokenType.Greater},
            {'!', TokenType.Not},
            {'(', TokenType.ParenthesesOpen},
            {')', TokenType.ParenthesesClose},
            {'{', TokenType.BraceCurlyOpen},
            {'}', TokenType.BraceCurlyClose},
        };

        private static readonly Dictionary<string, TokenType> LogicalOperators = new()
        {
            {"==", TokenType.EqualsEquals},
            {"<=", TokenType.LessEquals},
            {">=", TokenType.GreaterEquals},
            {"!=", TokenType.NotEquals}
        };

        private static readonly HashSet<char> LogicalOperatorsCharacters = LogicalOperators.Keys.Distinct().Select(s => s[0]).ToHashSet();

        private static readonly Dictionary<string, TokenType> SpecialWords = new()
        {
            {"for", TokenType.KeywordFor},
            {"while", TokenType.KeywordWhile},
            {"if", TokenType.KeywordIf},
            {"else", TokenType.KeywordElse}
        };

        public IEnumerable<IToken> Tokenize(string input)
        {
            int i = 0;

            while (i < input.Length)
            {
                if (char.IsWhiteSpace(input[i]))
                {
                    i++;
                }
                else if (SpecialCharacters.ContainsKey(input[i]))
                {
                    char value = input[i];
                    i++;
                    yield return new Token(value, SpecialCharacters[value]);
                }
                else if (char.IsDigit(input[i]))
                {
                    yield return ReadDigit(input, ref i);
                }
                else if (char.IsLetter(input[i]))
                {
                    IToken? specialCharactersGroup = TryReadOperator(input, ref i);
                    if (specialCharactersGroup != null)
                    {
                        yield return specialCharactersGroup;
                    }
                    else
                    {
                        yield return ReadSpecialWordOrIdentifier(input, ref i);
                    }
                }
                else
                {
                    throw new Exception($"Unexpected token '{input[i]}' at position {i}");
                }
            }

            yield return new Token(null, TokenType.EOF);
        }

        private static IToken ReadSpecialWordOrIdentifier(string input, ref int i)
        {
            int read = 0;

            while (char.IsLetterOrDigit(input[i + read]))
            {
                read++;
            }

            string value = input.Substring(i, read);
            i += read;

            if (SpecialWords.ContainsKey(value))
            {
                return new Token(value, SpecialWords[value]);
            }

            return new Token(value, TokenType.Identifier);
        }

        private static IToken ReadDigit(string input, ref int i)
        {
            int nextIndex = i + 1;
            while (nextIndex < input.Length && char.IsDigit(input[nextIndex]))
            {
                nextIndex++;
            }

            int read = nextIndex - i;
            string value = input.Substring(i, read);

            i += read;
            return new Token(value, TokenType.Number);
        }

        private static IToken? TryReadOperator(string input, ref int i)
        {
            int read = 0;
            while (LogicalOperatorsCharacters.Contains(input[i + read]))
            {
                read++;
            }

            string value = input.Substring(i, read);
            if (LogicalOperators.ContainsKey(value))
            {
                i += read;
                return new Token(value, LogicalOperators[value]);
            }

            return null;
        }
    }
}