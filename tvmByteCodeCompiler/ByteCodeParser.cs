using System;
using System.Text;

namespace tvmByteCodeCompiler
{
    public sealed class ByteCodeParser
    {
        private readonly string _stream;

        private int _position = 0;

        public int Line { get; private set; } = 1;

        public ByteCodeParser(string stream) => _stream = stream;

        public string NextCommand()
        {
            SkipWhiteSpaces();
            if (Eof()) return null;
            return ReadCommand();
        }

        public int NextValue()
        {
            SkipWhiteSpaces();
            if (Eof()) throw new InvalidOperationException($"ERROR: expected value at line {Line}");

            if (int.TryParse(ReadCommand(), out int value)) return value;
            else throw new ArgumentException($"ERROR: value at line {Line} is not 32bit integer number");
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
            while (!Eof() && char.IsWhiteSpace(_stream[_position]))
            {
                if (_stream[_position] == '\n') Line++;
                _position++;
            }
        }

        private bool Eof() => _stream.Length == _position;
    }
}
