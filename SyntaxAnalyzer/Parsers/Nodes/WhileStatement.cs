namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class WhileStatement : StatementNode
    {
        public ExpressionNode WhileCondition { get; set; }
        public StatementNode WhileBody { get; set; }
    }
}