using System.Text;
using System.Collections.Generic;
using tvmByteCodeCommands;

namespace tbcc
{
    public sealed class ByteCodeParser
    {
        private readonly string _stream;

        private int _position = 0;

        public ByteCodeParser(string stream) => _stream = stream;

        public byte[] Parse()
        {
            List<byte> byteCode = new();

            while (!Eof())
            {
                SkipWhiteSpaces();
            }
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
