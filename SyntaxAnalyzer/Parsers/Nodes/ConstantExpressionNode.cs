using SyntaxAnalyzer.Lexers;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class ConstantExpressionNode : ExpressionNode
    {
        public IToken Value { get; set; }
    }
}