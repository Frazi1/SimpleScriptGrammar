using System.Collections.Generic;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class ScriptNode : Node
    {
        public StatementListNode StatementList { get; set; }
    }
}