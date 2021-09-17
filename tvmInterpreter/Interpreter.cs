using System;

using tvmInterpreter.Memory;

namespace tvmInterpreter
{
    public sealed class Interpreter
    {
        public CommandMemory Commands { get; private set; }

        public StackMemory MemoryStack { get; private set; }

        private Interpreter() => MemoryStack = new();

        public Interpreter(byte[] byteCode) : base() => Commands = new(byteCode);

        public void Interpret()
        {

        }
    }
}
