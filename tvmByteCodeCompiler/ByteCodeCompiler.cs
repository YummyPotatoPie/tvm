using System;
using System.Collections.Generic;

using tvmByteCodeCommands;

namespace tvmByteCodeCompiler
{
    /// <summary>
    /// Compiler .tbc to .tbcc
    /// </summary>
    public sealed class ByteCodeCompiler
    {
        private readonly ByteCodeParser _parser;

        private readonly bool _strictMode = false;

        public ByteCodeCompiler(string sourceCode) => _parser = new(sourceCode);

        public ByteCodeCompiler(string sourceCode, bool strictMode) : this(sourceCode) => _strictMode = strictMode;

        /// <summary>
        /// Compile program to opcode representation
        /// </summary>
        /// <returns>Program compiled to opcode</returns>
        public byte[] Compile()
        {
            List<byte> compiledCode = new();

            string currentCommand = _parser.NextCommand();
            while (currentCommand != null)
            {
                if (ByteCodeCommands.CommandsWithArgument.ContainsKey(currentCommand))
                    AddBytes(ref compiledCode, CommandWithArgumentHandle(currentCommand));
                else if (ByteCodeCommands.CommandsWithoutArgument.ContainsKey(currentCommand))
                    compiledCode.Add(ByteCodeCommands.CommandsWithoutArgument[currentCommand]);
                else throw new Exception($"ERROR: unexpected value at line {_parser.Line}");
               
                currentCommand = _parser.NextCommand();
            }

            return compiledCode.ToArray();
        }

        /// <summary>
        /// Compile byte code command to opcode 
        /// </summary>
        /// <param name="currentCommand">Current program command</param>
        /// <returns>Opcode of command with argument</returns>
        private byte[] CommandWithArgumentHandle(string currentCommand)
        {
            List<byte> bytes = new();
            bytes.Add(ByteCodeCommands.CommandsWithArgument[currentCommand]);

            try
            {
                AddBytes(ref bytes, BitConverter.GetBytes(_parser.NextValue()));
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                if (_strictMode) throw;
                else
                {
                    Console.WriteLine($"WARNING: value at line {_parser.Line} is not 32bit integer number. Number value set to default (0)");
                    AddBytes(ref bytes, BitConverter.GetBytes(0));
                }
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ls">List of program opcode</param>
        /// <param name="bytes">Command opcode</param>
        private static void AddBytes(ref List<byte> ls, byte[] bytes)
        {
            foreach (byte b in bytes) ls.Add(b);
        }
    }
}
