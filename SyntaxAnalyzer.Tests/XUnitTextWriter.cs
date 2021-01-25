using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace SyntaxAnalyzer.Tests
{
    public class XUnitTextWriter : TextWriter
    {
        private readonly ITestOutputHelper _outputHelper;
        public override Encoding Encoding => Encoding.UTF8;

        public XUnitTextWriter(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;
        public override void Write(string? value) => _outputHelper.WriteLine(value);
    }
}