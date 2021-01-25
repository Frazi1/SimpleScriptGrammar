namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class LogicalExpressionNode : BinaryExpressionNode
    {
        public enum LogicalExpressionType
        {
            Greater,
            Less,
            Equals,
            GreaterEquals,
            LessEquals,
            NotEquals
        }

        public LogicalExpressionType Type { get; set; }
    }
}