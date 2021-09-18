using System.Collections.Generic;
using System.Text;

namespace tvmByteCodeParser
{
    public sealed class ByteCodeParser
    {
        private readonly string _stream;

        private int _position = 0;

        public ByteCodeParser(string stream) => _stream = stream;

        public string[] Parse()
        {
            List<string> byteCode = new();

            while (!Eof())
            {
                SkipWhiteSpaces();
                if (Eof()) break;
                byteCode.Add(ReadCommand());
            }

            return byteCode.ToArray();
        }

        private string ReadCommand()
        {
            StringBuilder command = new();

            while (!Eof() && !char.IsWhiteSpace(_stream[_position]))
            {
                command.Append(_stream[_position]);
                _position++;
            }

            return command.ToString();
        }

        private void SkipWhiteSpaces()
        {
            while (!Eof() && char.IsWhiteSpace(_stream[_position])) _position++;
        }

        private bool Eof() => _stream.Length == _position;
    }
}
