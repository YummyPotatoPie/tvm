using System;

namespace tvm
{
    public class CommandLineArgumentsParseException : Exception
    {
        public CommandLineArgumentsParseException(string message) : base(message) { }
    }
}
