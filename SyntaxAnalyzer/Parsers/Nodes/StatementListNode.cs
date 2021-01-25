using System.Collections.Generic;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class StatementListNode : StatementNode
    {
        public IList<StatementNode> Statements { get; set; } = new List<StatementNode>();
    }
}