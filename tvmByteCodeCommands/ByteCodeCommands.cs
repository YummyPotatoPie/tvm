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
            { "push",   0x1 },
            { "peek",   0x8 },
            { "preg",   0x9 },
            { "ptrfc",  0xA },
            { "vpop",   0xB },
            { "int",    0xC },
        };

        /// <summary>
        /// Commands that execute with arguments 
        /// </summary>
        public static Dictionary<string, byte> CommandsWithoutArgument { get; private set; } = new Dictionary<string, byte>
        {
            { "pop",    0x2 },
            { "add",    0x3 },
            { "sub",    0x4 },
            { "mul",    0x5 },
            { "div",    0x6 },
            { "dup",    0x7 }
        };
    }
}