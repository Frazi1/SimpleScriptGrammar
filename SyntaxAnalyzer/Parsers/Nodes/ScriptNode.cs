using System.Collections.Generic;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class ScriptNode : Node
    {
        public List<StatementNode> Statements { get; set; } = new();
    }
}