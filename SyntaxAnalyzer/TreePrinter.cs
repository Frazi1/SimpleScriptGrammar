using System;
using System.Collections.Generic;
using System.IO;
using SyntaxAnalyzer.Lexers;
using SyntaxAnalyzer.Parsers;
using SyntaxAnalyzer.Parsers.Nodes;

namespace SyntaxAnalyzer
{
    public class TreePrinter
    {
        private static string IncreaseIndent(string indent, bool isLast) => indent + (isLast ? "   " : "|  ");

        private static void PrintValue(TextWriter writer, string indent, string value) => writer.Write(indent + "+- " + value);
        private static void PrintValue(TextWriter writer, string indent, IToken token) => PrintValue(writer, indent, token.Value);


        public static void PrintRoot(TextWriter writer, ScriptNode root)
        {
            string indent = "";
            PrintValue(writer, indent, "root");
            PrintStatementList(writer, IncreaseIndent(indent ,true), root.StatementList);
        }

        private static void PrintStatementList(TextWriter writer, string indent, StatementListNode statementList)
        {
            PrintValue(writer, indent, "'statements'");

            var statements = statementList.Statements;
            if (statements.Count == 0)
            {
                PrintValue(writer, IncreaseIndent(indent, true), "'EMPTY'");
                return;
            }
            
            for (int i = 0; i < statements.Count; i++)
            {
                PrintStatement(writer, IncreaseIndent(indent, i + 1 == statements.Count), statements[i]);
            }
        }

        private static void PrintStatement(TextWriter writer, string indent, StatementNode statement)
        {
            switch (statement)
            {
                case AssignmentStatementNode assignmentStatementNode:
                    PrintValue(writer, indent, "'='");
                    PrintVariable(writer, IncreaseIndent(indent, false), assignmentStatementNode.Variable);
                    PrintExpression(writer, IncreaseIndent(indent, true), assignmentStatementNode.Expression);
                    break;
                case BlockStatementNode blockStatementNode:
                    PrintValue(writer, indent, "{");
                    PrintStatementList(writer, IncreaseIndent(indent, true), blockStatementNode.StatementList);
                    PrintValue(writer, indent, "}");
                    break;
                case IfElseStatement ifElseStatement:
                    PrintValue(writer, indent, "if");
                    PrintExpression(writer, IncreaseIndent(indent, false), ifElseStatement.IfExpression);
                    PrintStatement(writer, IncreaseIndent(indent, true), ifElseStatement.IfStatement);
                    if (ifElseStatement.ElseStatement != null)
                    {
                        PrintValue(writer, indent, "else");
                        PrintStatement(writer, IncreaseIndent(indent, true), ifElseStatement.ElseStatement);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statement), statement, "Not implemented");
            }
        }

        private static void PrintVariable(TextWriter writer, string indent, VariableNode variableNode)
        {
            PrintValue(writer, indent, $"Identifier: '{variableNode.Identifier.Value}'");
        }
        
        private static void PrintExpression(TextWriter writer, string indent, ExpressionNode expression)
        {
            switch (expression)
            {
                case AlgebraicGroupExpression algebraicGroupExpression:
                    PrintValue(writer, indent, "(");
                    PrintExpression(writer, IncreaseIndent(indent, true), algebraicGroupExpression.InnerExpression);
                    PrintValue(writer, indent, ")");
                    break;
                case MultiplyExpressionNode multiplyExpressionNode:
                    PrintValue(writer, indent, multiplyExpressionNode.Type switch
                    {
                        MultiplyExpressionNode.MultiplyType.Divide => "'/'",
                        MultiplyExpressionNode.MultiplyType.Multiply => "'*'"
                    });
                    PrintExpression(writer, IncreaseIndent(indent, false), multiplyExpressionNode.Left);
                    PrintExpression(writer, IncreaseIndent(indent, true), multiplyExpressionNode.Right);
                    break;
                case LogicalExpressionNode logicalExpressionNode:
                    PrintValue(writer, indent, logicalExpressionNode.Type switch
                    {
                        LogicalExpressionNode.LogicalExpressionType.Equals => "'=='",
                        LogicalExpressionNode.LogicalExpressionType.Greater => "'>'",
                        LogicalExpressionNode.LogicalExpressionType.Less => "'<'",
                        LogicalExpressionNode.LogicalExpressionType.GreaterEquals => "'>='",
                        LogicalExpressionNode.LogicalExpressionType.LessEquals => "'<='",
                        LogicalExpressionNode.LogicalExpressionType.NotEquals => "'!='",
                        LogicalExpressionNode.LogicalExpressionType.And => "'and'",
                        LogicalExpressionNode.LogicalExpressionType.Or => "'or'",
                    });
                    PrintExpression(writer, IncreaseIndent(indent, false), logicalExpressionNode.Left);
                    PrintExpression(writer, IncreaseIndent(indent, true), logicalExpressionNode.Right);
                    break;
                case SumExpressionNode sumExpressionNode:
                    PrintValue(writer, indent, sumExpressionNode.Type == SumExpressionNode.SumType.Plus ? "'+'" : "'-'");
                    PrintExpression(writer, IncreaseIndent(indent, false), sumExpressionNode.Left);
                    PrintExpression(writer, IncreaseIndent(indent, true), sumExpressionNode.Right);
                    break;
                case ConstantExpressionNode constantExpression:
                    PrintValue(writer, indent, constantExpression.Value);
                    break;
                case VariableNode variableNode:
                    PrintValue(writer, indent, variableNode.Identifier);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }
    }
}