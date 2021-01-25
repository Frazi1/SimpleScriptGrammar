namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class SumExpressionNode : BinaryExpressionNode
    {
        public enum SumType
        {
            Plus,
            Minus
        }

        public SumType Type { get; set; }
    }
}