using System;
using System.IO;
using System.Text;

namespace SyntaxAnalyzer
{
    public class ConsoleTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            Console.Write(value);
        }
    }
}