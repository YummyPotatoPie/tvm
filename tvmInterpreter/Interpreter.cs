using System;

using tvmByteCodeCommands;
using tvmInterpreter.Memory;

namespace tvmInterpreter
{
    public sealed class Interpreter
    {
        public CommandMemory Commands { get; private set; }

        public StackMemory MemoryStack { get; private set; }

        public RegisterMemory Registers { get; private set; }

        private Interpreter()
        {
            MemoryStack = new();
            Registers = new();
        }

        public Interpreter(byte[] byteCode) : this() => Commands = new(byteCode);

        public Interpreter(byte[] byteCode, int size) : this(byteCode) => MemoryStack = new(size);

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
            }
        }

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
            }
        }

        private void BinaryCommandHandle(Func<int, int, int> command)
        {
            int a = GetValueFromStack(), b = GetValueFromStack();
            MemoryStack.Push(command(b, a));
        }

        private int GetValueFromStack()
        {
            int value = MemoryStack.Peek(); MemoryStack.Pop();
            return value;
        }
    }
}
