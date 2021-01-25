using System.Collections.Generic;
using SyntaxAnalyzer.Lexers;
using SyntaxAnalyzer.Parsers.Nodes;

namespace SyntaxAnalyzer.Parsers
{
    public class StatementParser
    {
        private readonly ExpressionParser _expressionParser;

        public StatementParser(ExpressionParser expressionParser)
        {
            _expressionParser = expressionParser;
        }

        public StatementNode ParseStatement(Node parent, IEnumerator<IToken> enumerator)
        {
            return TryParseStatement(parent, enumerator) ?? throw new ExpectedNodeMissingException<StatementNode>(null);
        }

        public StatementNode? TryParseStatement(Node parent, IEnumerator<IToken> enumerator)
        {
            IToken? token = enumerator.Current;

            if (token is {Type: TokenType.EOF})
            {
                return null;
            }

            if (token is {Type: TokenType.BraceCurlyOpen})
            {
                var blockStatement = new BlockStatementNode {Parent = parent};

                enumerator.GetNextToken();
                blockStatement.StatementList = ParseStatementList(blockStatement, enumerator);

                enumerator.AssertToken(TokenType.BraceCurlyClose);
                return blockStatement;
            }

            if (token is {Type: TokenType.Identifier})
            {
                if (enumerator.GetNextToken() is {Type: TokenType.Equals})
                {
                    AssignmentStatementNode assignmentStatementNode = new() {Parent = parent};
                    VariableNode variableNode = new() {Parent = assignmentStatementNode, Identifier = token};
                    assignmentStatementNode.Variable = variableNode;

                    enumerator.GetNextToken();
                    ExpressionNode expression = _expressionParser.ParseExpression(assignmentStatementNode, enumerator);
                    assignmentStatementNode.Expression = expression;

                    return assignmentStatementNode;
                }
            }

            if (token is {Type: TokenType.KeywordIf})
            {
                IfElseStatement ifElseStatement = new() {Parent = parent};

                enumerator.GetNextToken();
                enumerator.AssertToken(TokenType.ParenthesesOpen);
                ifElseStatement.IfExpression = _expressionParser.ParseExpression(ifElseStatement, enumerator);
                enumerator.AssertToken(TokenType.ParenthesesClose);

                ifElseStatement.IfStatement = ParseStatement(ifElseStatement, enumerator);

                if (enumerator.Current is {Type: TokenType.KeywordElse})
                {
                    enumerator.GetNextToken();
                    ifElseStatement.ElseStatement = ParseStatement(ifElseStatement, enumerator);
                }

                return ifElseStatement;
            }

            if (token is {Type: TokenType.KeywordWhile})
            {
                WhileStatement whileStatement = new() {Parent = parent};

                enumerator.GetNextToken();

                enumerator.AssertToken(TokenType.ParenthesesOpen);
                whileStatement.WhileCondition = _expressionParser.ParseExpression(whileStatement, enumerator);
                enumerator.AssertToken(TokenType.ParenthesesClose);

                whileStatement.WhileBody = ParseStatement(whileStatement, enumerator);

                return whileStatement;
            }

            return null;
        }

        public StatementListNode ParseStatementList(Node parent, IEnumerator<IToken> enumerator)
        {
            var statementList = new StatementListNode() {Parent = parent};
            while (true)
            {
                StatementNode? statement = TryParseStatement(parent, enumerator);
                if (statement == null) break;

                statementList.Statements.Add(statement);
            }

            return statementList;
        }
    }
}