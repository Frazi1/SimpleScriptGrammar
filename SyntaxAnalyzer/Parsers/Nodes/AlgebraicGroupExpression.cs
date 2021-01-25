namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class AlgebraicGroupExpression : ExpressionNode
    {
        public ExpressionNode InnerExpression { get; set; }
    }
}