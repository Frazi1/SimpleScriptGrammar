using System;
using System.Collections.Generic;
using SyntaxAnalyzer.Lexers;
using SyntaxAnalyzer.Parsers.Nodes;

namespace SyntaxAnalyzer.Parsers
{
    public class Parser
    {
        public ScriptNode ParseTree(IEnumerable<IToken> tokens)
        {
            using var enumerator = tokens.GetEnumerator();

            ScriptNode scriptNode = ParseScript(enumerator);
            return scriptNode;
        }

        private ScriptNode ParseScript(IEnumerator<IToken> enumerator)
        {
            var script = new ScriptNode();

            enumerator.GetNextToken();

            script.StatementList = ParseStatementList(script, enumerator);

            return script;
        }

        private StatementListNode ParseStatementList(Node parent, IEnumerator<IToken> enumerator)
        {
            var statementList = new StatementListNode() {Parent = parent};
            while (true)
            {
                StatementNode? statement = TryParseLineStatement(parent, enumerator);
                if (statement == null) break;

                statementList.Statements.Add(statement);
            }

            return statementList;
        }


        private StatementNode ParseStatement(Node parent, IEnumerator<IToken> enumerator)
        {
            return TryParseLineStatement(parent, enumerator) ?? throw new ExpectedNodeMissingException<StatementNode>(null);
        }

        private StatementNode? TryParseLineStatement(Node parent, IEnumerator<IToken> enumerator)
        {
            IToken? token = enumerator.Current;

            if (token is {Type: TokenType.EOF})
            {
                return null;
            }

            if (enumerator.Current is {Type: TokenType.BraceCurlyOpen})
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
                    ExpressionNode expression = ParseExpression(assignmentStatementNode, enumerator);
                    assignmentStatementNode.Expression = expression;

                    return assignmentStatementNode;
                }
            }

            if (token is {Type: TokenType.KeywordIf})
            {
                IfElseStatement ifElseStatement = new() {Parent = parent};

                enumerator.GetNextToken();
                enumerator.AssertToken(TokenType.ParenthesesOpen);
                ifElseStatement.IfExpression = ParseExpression(ifElseStatement, enumerator);
                enumerator.AssertToken(TokenType.ParenthesesClose);

                ifElseStatement.IfStatement = ParseStatement(ifElseStatement, enumerator);

                if (enumerator.Current is {Type: TokenType.KeywordElse})
                {
                    enumerator.GetNextToken();
                    ifElseStatement.ElseStatement = ParseStatement(ifElseStatement, enumerator);
                }

                return ifElseStatement;
            }

            return null;
        }

        private ExpressionNode ParseTerm(Node parent, IEnumerator<IToken> enumerator)
        {
            var token = enumerator.Current;

            if (token is {Type: TokenType.Number or TokenType.KeywordFalse or TokenType.KeywordTrue})
            {
                enumerator.GetNextToken();
                return new ConstantExpressionNode {Parent = parent, Value = token};
            }

            if (token is {Type: TokenType.Identifier})
            {
                enumerator.GetNextToken();
                return new VariableNode {Parent = parent, Identifier = token};
            }

            if (token is {Type: TokenType.ParenthesesOpen})
            {
                enumerator.GetNextToken();
                AlgebraicGroupExpression groupExpression = new() {Parent = parent};

                groupExpression.InnerExpression = ParseExpression(groupExpression, enumerator);
                enumerator.AssertToken(TokenType.ParenthesesClose);

                return groupExpression;
            }

            throw new NotImplementedException($"Unrecognized token '{token.Value}' ({token.Type})");
        }

        private ExpressionNode ParseExpression(Node parent, IEnumerator<IToken> enumerator)
        {
            return ParseLogicalExpression(parent, enumerator);

            throw new NotImplementedException();
        }

        private ExpressionNode ParseGenericBinaryExpression<TNew>(Node parent, IEnumerator<IToken> enumerator,
            Func<Node, IEnumerator<IToken>, ExpressionNode> innerParser,
            Func<IToken, bool> tokenGuard,
            Func<IToken, TNew> createBinaryExpr)
            where TNew : BinaryExpressionNode
        {
            ExpressionNode left = innerParser(parent, enumerator);

            while (tokenGuard(enumerator.Current))
            {
                var binaryNode = createBinaryExpr(enumerator.Current);
                binaryNode.Parent = left.Parent;
                binaryNode.Left = left;


                enumerator.GetNextToken();
                binaryNode.Right = innerParser(binaryNode, enumerator);

                left.Parent = binaryNode;
                left = binaryNode;
            }

            return left;
        }

        private ExpressionNode ParseSum(Node parent, IEnumerator<IToken> enumerator)
        {
            return ParseGenericBinaryExpression(parent,
                enumerator,
                ParseTerm,
                token => token is {Type: TokenType.SignMinus or TokenType.SignPlus},
                token => new SumExpressionNode {Type = token.Type is TokenType.SignMinus ? SumExpressionNode.SumType.Minus : SumExpressionNode.SumType.Plus});
        }

        private ExpressionNode ParseMultiplicationDivision(Node parent, IEnumerator<IToken> enumerator)
        {
            return ParseGenericBinaryExpression(parent,
                enumerator,
                ParseSum,
                token => token is {Type: TokenType.SignMultiply or TokenType.SignDivide},
                token => new MultiplyExpressionNode
                {
                    Type = token.Type == TokenType.SignMultiply ? MultiplyExpressionNode.MultiplyType.Multiply : MultiplyExpressionNode.MultiplyType.Divide
                });
        }


        private ExpressionNode ParseLogicalOrExpression(Node parent, IEnumerator<IToken> enumerator)
        {
            return ParseGenericBinaryExpression(parent,
                enumerator,
                ParseMultiplicationDivision,
                token => token is {Type: TokenType.Or},
                _ => new LogicalExpressionNode {Type = LogicalExpressionNode.LogicalExpressionType.Or}
            );
        }

        private ExpressionNode ParseLogicalAndExpression(Node parent, IEnumerator<IToken> enumerator)
        {
            return ParseGenericBinaryExpression(parent,
                enumerator,
                ParseLogicalOrExpression,
                token => token is {Type: TokenType.And},
                _ => new LogicalExpressionNode {Type = LogicalExpressionNode.LogicalExpressionType.And}
            );
        }

        private ExpressionNode ParseLogicalExpression(Node parent, IEnumerator<IToken> enumerator)
        {
            return ParseGenericBinaryExpression(parent,
                enumerator,
                ParseLogicalAndExpression,
                token => token is
                {
                    Type: TokenType.EqualsEquals or TokenType.NotEquals or TokenType.Less or TokenType.Greater or TokenType.GreaterEquals or TokenType
                        .LessEquals
                },
                token => new LogicalExpressionNode
                {
                    Type = token.Type switch
                    {
                        TokenType.EqualsEquals => LogicalExpressionNode.LogicalExpressionType.Equals,
                        TokenType.Less => LogicalExpressionNode.LogicalExpressionType.Less,
                        TokenType.Greater => LogicalExpressionNode.LogicalExpressionType.Greater,
                        TokenType.GreaterEquals => LogicalExpressionNode.LogicalExpressionType.GreaterEquals,
                        TokenType.LessEquals => LogicalExpressionNode.LogicalExpressionType.LessEquals,
                        TokenType.NotEquals => LogicalExpressionNode.LogicalExpressionType.NotEquals
                    }
                });
        }
    }
}