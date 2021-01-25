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
            NotEquals,
            And,
            Or
        }

        public LogicalExpressionType Type { get; set; }
    }
}