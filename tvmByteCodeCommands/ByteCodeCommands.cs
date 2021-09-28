using System;
using System.Collections.Generic;

namespace tvmByteCodeCommands
{
    /// <summary>
    /// Tiny Byte Code commands
    /// </summary>
    public static class ByteCodeCommands
    {
        /// <summary>
        /// Commands that execute without argument 
        /// </summary>
        public static Dictionary<string, byte> CommandsWithArgument { get; private set; } = new Dictionary<string, byte>
        {
            { "push",   0x1  },
            { "peek",   0x8  },
            { "preg",   0x9  },
            { "ptrfr",  0xA  },
            { "vpop",   0xB  },
            { "int",    0x14 },
            { "ptrfw",  0x1A }
        };

        /// <summary>
        /// Commands that execute with arguments 
        /// </summary>
        public static Dictionary<string, byte> CommandsWithoutArgument { get; private set; } = new Dictionary<string, byte>
        {
            { "pop",    0x2  },
            { "add",    0x3  },
            { "sub",    0x4  },
            { "mul",    0x5  },
            { "div",    0x6  },
            { "dup",    0x7  },
            { "cmp",    0xC  },
            { "shiftr", 0xD  },
            { "shiftl", 0xE  },
            { "xor",    0xF  },
            { "or",     0x10 },
            { "and",    0x11 }
        };

        /// <summary>
        /// Jump commands
        /// </summary>
        public static Dictionary<string, byte> JumpCommands { get; private set; } = new Dictionary<string, byte>
        {
            { "jl",     0x15 },
            { "jb",     0x16 },
            { "je",     0x17 },
            { "jne",    0x18 },
            { "jmp",    0x19 }
        };

        /// <summary>
        /// Special commands of Tiny Byte Code
        /// </summary>
        public static Dictionary<string, byte> SpecialCommands { get; private set; } = new Dictionary<string, byte>
        {
            { "label",  0x12 },
            { "call",   0x13 },
        };
    }
}