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

        private int _currentCommandIndex = 0;

        public ByteCodeCompiler(string sourceCode) => _parser = new(sourceCode);

        public ByteCodeCompiler(string sourceCode, bool strictMode) : this(sourceCode) => _strictMode = strictMode;

        /// <summary>
        /// Compile program to opcode representation
        /// </summary>
        /// <returns>Program compiled to opcode</returns>
        public byte[] Compile() => SecondPhase(FirstPhase());

        /// <summary>
        /// First phase of compilation
        /// </summary>
        /// <returns>Label - address dictionary</returns>
        private Dictionary<string, int> FirstPhase()
        {
            Dictionary<string, int> labels = new();
            string currentCommand = _parser.NextCommand();

            while (currentCommand != null)
            {
                if (ByteCodeCommands.CommandsWithArgument.ContainsKey(currentCommand))
                {
                    _currentCommandIndex += 5;
                    try
                    {
                        _parser.NextValue();
                    }
                    catch (InvalidOperationException)
                    {
                        throw;
                    }
                }
                else if (ByteCodeCommands.CommandsWithoutArgument.ContainsKey(currentCommand))
                {
                    _currentCommandIndex++;
                }
                else if (ByteCodeCommands.JumpCommands.ContainsKey(currentCommand))
                {
                    _currentCommandIndex += 5;
                    string labelName = _parser.NextCommand();

                    if (labelName == null) throw new InvalidOperationException($"ERROR: '{currentCommand}' command must contain label name as argument");
                }
                else if (ByteCodeCommands.SpecialCommands.ContainsKey(currentCommand))
                {
                    FirstPhaseSpecialCommandsHandler(ref labels, currentCommand);
                }
                else throw new Exception($"ERROR: unexpected value or command at line {_parser.Line}");

                currentCommand = _parser.NextCommand();
            }

            _parser.Reset();

            return labels;
        }

        /// <summary>
        /// Special method to handle special commands 
        /// </summary>
        /// <param name="labels">Label - address dictionary</param>
        private void FirstPhaseSpecialCommandsHandler(ref Dictionary<string, int> labels, string currentCommand)
        {
            byte commandOpcode = ByteCodeCommands.SpecialCommands[currentCommand];
            
            switch (commandOpcode)
            {
                case 0x12:
                    LabelHandler(ref labels);
                    break;
                case 0x13:
                    _currentCommandIndex += 5;
                    string labelName = _parser.NextCommand();

                    if (labelName == null) throw new InvalidOperationException("ERROR: 'call' command must contain label name as argument");
                    break;
            }
        }

        /// <summary>
        /// Handle label command
        /// </summary>
        /// <param name="labels">Label - address dictionary</param>
        private void LabelHandler(ref Dictionary<string, int> labels)
        {
            string labelName = _parser.NextCommand();
            if (labelName == null) throw new InvalidOperationException($"ERROR: 'label' command must contains label name as argument, {_parser.Line}");
            if (!char.IsLetter(labelName[0])) throw new ArgumentException($"ERROR: label name must start with the letter, line {_parser.Line}");

            string nextCommand = _parser.ReadNextCommand();
            if (nextCommand == null) throw new InvalidOperationException("ERROR: after 'label' command must be at least one command");

            labels.Add(labelName, _currentCommandIndex);
        }

        /// <summary>
        /// Second phase of compilation
        /// </summary>
        /// <returns>Compiled code of Tiny Byte Code</returns>
        private byte[] SecondPhase(Dictionary<string, int> labels)
        {
            List<byte> compiledCode = new();
            string currentCommand = _parser.NextCommand();

            while (currentCommand != null)
            {
                if (ByteCodeCommands.CommandsWithArgument.ContainsKey(currentCommand))
                    AddBytes(ref compiledCode, CommandWithArgumentHandle(currentCommand));
                else if (ByteCodeCommands.CommandsWithoutArgument.ContainsKey(currentCommand))
                    compiledCode.Add(ByteCodeCommands.CommandsWithoutArgument[currentCommand]);
                else if (ByteCodeCommands.JumpCommands.ContainsKey(currentCommand))
                    AddBytes(ref compiledCode, JumpCommandsHandler(currentCommand, ref labels));
                else if (ByteCodeCommands.SpecialCommands.ContainsKey(currentCommand))
                    AddBytes(ref compiledCode, SpecialCommandsHandler(currentCommand, ref labels));
                else throw new Exception($"ERROR: unexpected value or command at line {_parser.Line}");

                currentCommand = _parser.NextCommand();
            }

            return compiledCode.ToArray();
        }

        /// <summary>
        /// Compile special commands
        /// </summary>
        /// <param name="currentCommand">Current command name</param>
        /// <param name="labels">Label - address dictionary</param>
        /// <returns>Opcode of special command</returns>
        private byte[] SpecialCommandsHandler(string currentCommand, ref Dictionary<string, int> labels)
        {
            List<byte> bytes = new();
            byte currentOpcode = ByteCodeCommands.SpecialCommands[currentCommand];

            if (currentOpcode == 0x12) _parser.NextCommand();
            else if (currentOpcode == 0x13)
            {
                string labelName = _parser.NextCommand();

                if (!labels.ContainsKey(labelName)) throw new ArgumentException($"ERROR: unknown label address at line {_parser.Line}");

                bytes.Add(currentOpcode);
                AddBytes(ref bytes, BitConverter.GetBytes(labels[labelName]));
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// Compile jump commands
        /// </summary>
        /// <param name="currentCommand">Current command name</param>
        /// <param name="labels">Label - address dictionary</param>
        /// <returns>Opcode of jump command</returns>
        private byte[] JumpCommandsHandler(string currentCommand, ref Dictionary<string, int> labels)
        {
            List<byte> bytes = new();
            byte currentOpcode = ByteCodeCommands.JumpCommands[currentCommand];
            string labelName = _parser.NextCommand();

            if (!labels.ContainsKey(labelName)) throw new ArgumentException($"ERROR: unknown label address at line {_parser.Line}");

            bytes.Add(currentOpcode);
            AddBytes(ref bytes, BitConverter.GetBytes(labels[labelName]));

            return bytes.ToArray();
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
        /// Add opcode to program bytes
        /// </summary>
        /// <param name="ls">List of program opcode</param>
        /// <param name="bytes">Command opcode</param>
        private static void AddBytes(ref List<byte> ls, byte[] bytes)
        {
            foreach (byte b in bytes) ls.Add(b);
        }
    }
}
