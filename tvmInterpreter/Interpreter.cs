using System;

using tvmByteCodeCommands;
using tvmInterpreter.Memory;

namespace tvmInterpreter
{
    /// <summary>
    /// Interpreter fo Tiny Byte Code
    /// </summary>
    public sealed class Interpreter
    {
        public CommandMemory Commands { get; private set; }

        public StackMemory MemoryStack { get; private set; }

        public RegisterMemory Registers { get; private set; }

        public Flags ExecuteFlags { get; private set; }

        /// <summary>
        /// Constructor for create instance of MemoryStack and Registers
        /// </summary>
        private Interpreter()
        {
            MemoryStack = new();
            Registers = new();
            ExecuteFlags = new();
        }

        /// <summary>
        /// Constructor sets stack size to default value 1024 integers number
        /// </summary>
        /// <param name="byteCode">Tiny Byte Code commands</param>
        public Interpreter(byte[] byteCode) : this() => Commands = new(byteCode);

        /// <summary>
        /// Constructor sets stack size to default value 1024 integers number
        /// </summary>
        /// <param name="byteCode">Tiny Byte Code commands</param>
        /// <param name="size">Stack size</param>
        public Interpreter(byte[] byteCode, int size) : this(byteCode) => MemoryStack = new(size);

        /// <summary>
        /// Interpret Tiny Byte Code
        /// </summary>
        public void Interpret()
        {
            while (!Commands.EndOfProgram())
            {
                byte currentCommand = Commands.GetCurrentCommand();
                Commands.Next();

                if (ByteCodeCommands.CommandsWithArgument.ContainsValue(currentCommand))
                {
                    CommandWithArgumentHandle(currentCommand);
                }
                else if (ByteCodeCommands.CommandsWithoutArgument.ContainsValue(currentCommand))
                {
                    CommandWithoutArgumentHandle(currentCommand);
                }
                else throw new ArgumentException("Invalid command or value");
            }
        }

        /// <summary>
        /// Handle command without arguments
        /// </summary>
        /// <param name="command">Current command</param>
        private void CommandWithoutArgumentHandle(byte command)
        {
            switch (command)
            {
                case 0x2:
                    MemoryStack.Pop();
                    break;
                case 0x3:
                    BinaryCommandHandle(BinaryCommands.Add);
                    break;
                case 0x4:
                    BinaryCommandHandle(BinaryCommands.Sub);
                    break;
                case 0x5:
                    BinaryCommandHandle(BinaryCommands.Mul);
                    break;
                case 0x6:
                    BinaryCommandHandle(BinaryCommands.Div);
                    break;
                case 0x7:
                    MemoryStack.Push(MemoryStack.Peek());
                    break;
                case 0xD:
                    int first = MemoryStack.Peek(-1), second = MemoryStack.Peek();
                    if (first >= second) ExecuteFlags.CompareFlag = CompareFlags.Bigger;
                    else if (first <= second) ExecuteFlags.CompareFlag = CompareFlags.Lower;
                    else ExecuteFlags.CompareFlag = CompareFlags.Equal;
                    break;
                case 0xE:
                    BinaryCommandHandle(BinaryCommands.ShiftRight);
                    break;
                case 0xF:
                    BinaryCommandHandle(BinaryCommands.ShiftLeft);
                    break;
            }
        }

        /// <summary>
        /// Handle command with argument
        /// </summary>
        /// <param name="command">Current command</param>
        private void CommandWithArgumentHandle(byte command)
        {
            switch (command)
            {
                case 0x1:
                    MemoryStack.Push(Commands.GetValue());
                    break;
                case 0x8:
                    Registers.SetRegisterValue(Commands.GetValue(), MemoryStack.Peek());
                    break;
                case 0x9:
                    MemoryStack.Push(Registers.GetRegisterValue(Commands.GetValue()));
                    break;
                case 0xA:
                    int offset = Commands.GetValue();
                    MemoryStack.Push(MemoryStack.Peek(offset));
                    break;
                case 0xB:
                    int countOfValues = Commands.GetValue();
                    for (int i = 0; i < countOfValues; i++) MemoryStack.Pop();
                    break;
                //case 0xC:
            }
        }

        /// <summary>
        /// Execute binary commands
        /// </summary>
        /// <param name="command">Current command</param>
        private void BinaryCommandHandle(Func<int, int, int> command)
        {
            int a = GetValueFromStack(), b = GetValueFromStack();
            MemoryStack.Push(command(b, a));
        }

        /// <summary>
        /// Gets value from stack top with pop it
        /// </summary>
        /// <returns>Value from stack top </returns>
        private int GetValueFromStack()
        {
            int value = MemoryStack.Peek(); MemoryStack.Pop();
            return value;
        }
    }
}
