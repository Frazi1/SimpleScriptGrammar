using SyntaxAnalyzer.Parsers.Nodes;

namespace SyntaxAnalyzer.Parsers
{
    public class MultiplyExpressionNode : BinaryExpressionNode
    {
        public enum MultiplyType
        {
            Multiply,
            Divide
        }

        public MultiplyType Type { get; set; }
    }
}