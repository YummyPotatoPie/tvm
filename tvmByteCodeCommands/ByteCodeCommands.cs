using System;
using System.Collections.Generic;

namespace tvmByteCodeCommands
{
    public static class ByteCodeCommands
    {
        public static Dictionary<string, byte> Commands { get; private set; } = new Dictionary<string, byte>
        {
            { "push",   0x1 },
            { "pop",    0x2 },
            { "add",    0x3 },
            { "sub",    0x4 },
            { "mul",    0x5 },
            { "div",    0x6 }
        };
    }
}