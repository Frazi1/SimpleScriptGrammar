using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using SyntaxAnalyzer;
using SyntaxAnalyzer.ConsoleApp;
using SyntaxAnalyzer.Lexers;
using SyntaxAnalyzer.Parsers;
using SyntaxAnalyzer.Parsers.Nodes;


var logger = new LoggerConfiguration()
    .WriteTo.Console().MinimumLevel.Verbose()
    .CreateLogger();

try
{
    Run(args);
}

catch (Exception e)
{
    logger.Fatal(e, "Unexpected error");
}

ScriptNode ParseText(string s)
{
    IEnumerable<IToken> tokens = new Lexer().Tokenize(s);
    ScriptNode scriptNode = new Parser().ParseTree(tokens);
    return scriptNode;
}

void Run(string[] strings)
{
    string file = strings[0];
    string text = File.ReadAllText(file);

    ScriptNode tree = ParseText(text);

    TreePrinter.PrintRoot(new ConsoleTextWriter(), tree);
}