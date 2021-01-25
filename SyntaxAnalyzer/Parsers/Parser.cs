using System.Collections.Generic;
using SyntaxAnalyzer.Lexers;
using SyntaxAnalyzer.Parsers.Nodes;

namespace SyntaxAnalyzer.Parsers
{
    public class Parser
    {
        private readonly StatementParser _statementParser = new(new ExpressionParser());

        public ScriptNode ParseTree(IEnumerable<IToken> tokens)
        {
            using var enumerator = tokens.GetEnumerator();

            ScriptNode scriptNode = ParseScript(enumerator);
            return scriptNode;
        }

        private ScriptNode ParseScript(IEnumerator<IToken> enumerator)
        {
            var script = new ScriptNode();

            enumerator.GetNextToken();

            script.StatementList = _statementParser.ParseStatementList(script, enumerator);

            return script;
        }
    }
}