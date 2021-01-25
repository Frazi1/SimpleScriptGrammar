namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class IfElseStatement : StatementNode
    {
        public ExpressionNode IfExpression { get; set; }
        public StatementNode IfStatement { get; set; }

        public StatementNode? ElseStatement { get; set; }
    }
}