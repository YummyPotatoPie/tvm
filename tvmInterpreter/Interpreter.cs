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
                else if (ByteCodeCommands.JumpCommands.ContainsValue(currentCommand))
                {
                    JumpCommandHandle(currentCommand);
                }
                else if (ByteCodeCommands.SpecialCommands.ContainsValue(currentCommand))
                {
                    SpecialCommandHandle(currentCommand);
                }
                else throw new ArgumentException("Invalid command or value");
            }
        }

        /// <summary>
        /// Handle special commands 
        /// </summary>
        /// <param name="command">Current command</param>
        private void SpecialCommandHandle(byte command)
        {
            switch (command)
            {
                case 0x13:
                    int address = Commands.GetValue();
                    Commands.TransferControl(address);
                    break;
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
                case 0xC:
                    int first = MemoryStack.Peek(-1), second = MemoryStack.Peek();
                    if (first > second) ExecuteFlags.CompareFlag = CompareFlags.Bigger;
                    else if (first < second) ExecuteFlags.CompareFlag = CompareFlags.Lower;
                    else ExecuteFlags.CompareFlag = CompareFlags.Equal;
                    break;
                case 0xD:
                    BinaryCommandHandle(BinaryCommands.ShiftRight);
                    break;
                case 0xE:
                    BinaryCommandHandle(BinaryCommands.ShiftLeft);
                    break;
                case 0xF:
                    BinaryCommandHandle(BinaryCommands.Xor);
                    break;
                case 0x10:
                    BinaryCommandHandle(BinaryCommands.Or);
                    break;
                case 0x11:
                    BinaryCommandHandle(BinaryCommands.And);
                    break;
                case 0x1B:
                    Commands.ReturnControl();
                    break;
                case 0x1C:
                    BinaryCommandHandle(BinaryCommands.Mod);
                    break;
                case 0x1D:
                    int addressToRead = MemoryStack.Peek();
                    MemoryStack.Pop();
                    MemoryStack.Push(MemoryStack.PeekValueAddressed(addressToRead));
                    break;
                case 0x1F:
                    int addressToWriteValue = MemoryStack.Peek();
                    MemoryStack.Pop();
                    int valueToWrite = MemoryStack.Peek();
                    MemoryStack.Pop();
                    MemoryStack.SetValueAddressed(valueToWrite, addressToWriteValue);
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
                case 0x1A:
                    MemoryStack.SetValue(MemoryStack.Peek(), Commands.GetValue());
                    break;
                case 0x14:
                    int value = Commands.GetValue();
                    InterruptHandle(value);
                    break;
                case 0x1E:
                    int reserveSize = Commands.GetValue();
                    for (int i = 0; i < reserveSize; i++) MemoryStack.Push(0);
                    break;
            }
        }

        /// <summary>
        /// Handles interruptions
        /// </summary>
        /// <param name="interruptNumber">Interrupt number</param>
        private void InterruptHandle(int interruptNumber)
        {
            switch (interruptNumber)
            {
                case 1:
                    Console.Write((char)MemoryStack.Peek());
                    break;
                case 2:
                    try
                    {
                        MemoryStack.Push(Convert.ToInt32(Console.ReadLine()));
                    }
                    catch
                    {
                        Console.WriteLine("\nInvalid number format");
                        Environment.Exit(0);
                    }
                    break;
            }
        }

        /// <summary>
        /// Jump commands handler
        /// </summary>
        /// <param name="command">Jump command opcode</param>
        private void JumpCommandHandle(byte command)
        {
            int address = Commands.GetValue();
            switch (command)
            {
                case 0x15:
                    if (ExecuteFlags.CompareFlag == CompareFlags.Lower) Commands.SetPointer(address);
                    break;
                case 0x16:
                    if (ExecuteFlags.CompareFlag == CompareFlags.Bigger) Commands.SetPointer(address);
                    break;
                case 0x17:
                    if (ExecuteFlags.CompareFlag == CompareFlags.Equal) Commands.SetPointer(address);
                    break;
                case 0x18:
                    if (ExecuteFlags.CompareFlag != CompareFlags.Equal) Commands.SetPointer(address);
                    break;
                case 0x19:
                    Commands.SetPointer(address);
                    break;
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
