namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class BlockStatementNode : StatementNode
    {
        public StatementListNode StatementList { get; set; }
    }
}