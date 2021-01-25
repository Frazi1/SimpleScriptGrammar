namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class AssignmentStatementNode : StatementNode
    {
        public VariableNode Variable { get; set; }
        public ExpressionNode Expression { get; set; }
    }
}