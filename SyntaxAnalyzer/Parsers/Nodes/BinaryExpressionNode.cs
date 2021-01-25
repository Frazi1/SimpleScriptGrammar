namespace SyntaxAnalyzer.Parsers.Nodes
{
    public abstract class BinaryExpressionNode : ExpressionNode
    {
        public ExpressionNode Left { get; set; }
        public ExpressionNode Right { get; set; }
    }
}