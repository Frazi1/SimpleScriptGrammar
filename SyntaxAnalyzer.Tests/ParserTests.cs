using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SyntaxAnalyzer.Parsers;
using Xunit;
using Xunit.Abstractions;

namespace SyntaxAnalyzer.Tests
{
    public class ParserTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private Parser Parser => new();
        private Lexer Lexer => new();

        public ParserTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        private static IEnumerable<(string input, string[] assertTokens)> GetInputs()
        {
            yield return ("a = 15", new[] {"a", "=", "15"});
            yield return ("a = 1 + 2", new[] {"a", "=", "1", "+", "2"});
            yield return ("a = 1 - 2", new[] {"a", "=", "1", "-", "2"});
            yield return ("a = 1 - 2 + 3", new[] {"a", "=", "1", "-", "2", "+", "3"});
            yield return ("a = 1 - 2 * 3", new[] {"a", "=", "1", "-", "2", "*", "3"});
            yield return ("a = 1 * 2", new[] {"a", "=", "1", "*", "2"});
            yield return ("a = 1 / 2", new[] {"a", "=", "1", "/", "2"});
            yield return ("boolTest = a < 15", new[] {"boolTest", "=", "a", "<", "15"});
            yield return (@"
if(a < 15)
{
    a = 15 * 15 - 2 +1
}
else
{
    a = 56
}
", new[] {"if", "(", "a", "<", "15", ")", "{", "a", "=", "15", "*", "15", "-", "2", "+", "1", "}", "else", "{", "a", "=", "56", "}"});
        }

        public static IEnumerable<object[]> GetData() => GetInputs().Select(x => new object[] {x.input, x.assertTokens});

        [Theory]
        [MemberData(nameof(GetData))]
        public void AllParserTests(string input, string[] assertTokens)
        {
            var tokens = Lexer.Tokenize(input).ToArray();
            tokens.Select(t => t.Value).Should().BeEquivalentTo(assertTokens.Append(null));

            var root = Parser.ParseTree(tokens);

            var textWriter = new XUnitTextWriter(_outputHelper);

            TreePrinter.PrintRoot(textWriter, root);
        }
    }
}