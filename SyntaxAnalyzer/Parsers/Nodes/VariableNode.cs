using SyntaxAnalyzer.Lexers;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class VariableNode : ExpressionNode
    {
        public IToken Identifier { get; set; }
    }
}