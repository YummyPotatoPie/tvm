using System;
using System.Text;

namespace tvmByteCodeCompiler
{
    /// <summary>
    /// Parser for .tbc files
    /// </summary>
    public sealed class ByteCodeParser
    {
        private readonly string _stream;

        private int _position = 0;

        public int Line { get; private set; } = 1;

        public ByteCodeParser(string stream) => _stream = stream;

        /// <summary>
        /// Reads next command at the stream
        /// </summary>
        /// <returns>Next command at the stream</returns>
        public string NextCommand()
        {
            SkipWhiteSpaces();
            if (Eof()) return null;
            if (_stream[_position] == '#')
            {
                SkipComment();
                if (Eof()) return null;
            }
            return ReadCommand();
        }

        /// <summary>
        /// Reads next command with save stream pointer
        /// </summary>
        /// <returns>Next command</returns>
        public string ReadNextCommand()
        {
            int pointer = _position;
            string command = NextCommand();

            _position = pointer;
            return command;
        }

        /// <summary>
        /// Reads next number
        /// </summary>
        /// <returns>Next number at the stream</returns>
        public int NextValue()
        {
            SkipWhiteSpaces();
            if (Eof()) throw new InvalidOperationException($"ERROR: expected value at line {Line}");

            if (int.TryParse(ReadCommand(), out int value)) return value;
            else throw new ArgumentException($"ERROR: value at line {Line} is not 32bit integer number");
        }

        /// <summary>
        /// Sets stream pointer to start
        /// </summary>
        public void Reset()
        {
            _position = 0;
            Line = 1;
        }

        /// <summary>
        /// Reads current command at the stream
        /// </summary>
        /// <returns>Current command at the stream</returns>
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

        /// <summary>
        /// Skips whitespaces
        /// </summary>
        private void SkipWhiteSpaces()
        {
            while (!Eof() && char.IsWhiteSpace(_stream[_position]))
            {
                if (_stream[_position] == '\n') Line++;
                _position++;
            }
        }

        /// <summary>
        /// Skips comment
        /// </summary>
        private void SkipComment()
        {
            while (!Eof() && _stream[_position] == '#')
            {
                while (!Eof() && _stream[_position] != '\n') _position++;

                SkipWhiteSpaces();
                if (!Eof() && _stream[_position] != '#') break;
            }
        }

        /// <summary>
        /// Checks if stream reached end 
        /// </summary>
        /// <returns>True if reached end of stream else false</returns>
        private bool Eof() => _stream.Length == _position;
    }
}
