using System;

namespace SyntaxAnalyzer.Parsers.Nodes
{
    public class ExpectedNodeMissingException<TExpected> : Exception
    {
        public ExpectedNodeMissingException(Node? actual) : base($"Expected node of type {typeof(TExpected)}, but received {actual?.GetType()}")
        {
        }
    }
}