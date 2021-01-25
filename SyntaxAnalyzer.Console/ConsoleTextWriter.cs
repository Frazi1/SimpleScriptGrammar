using System;
using System.IO;
using System.Text;

namespace SyntaxAnalyzer.ConsoleApp
{
    public class ConsoleTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(string? value)
        {
            Console.WriteLine(value);
        }
    }
}